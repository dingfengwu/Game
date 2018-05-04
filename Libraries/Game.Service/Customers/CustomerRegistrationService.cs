using System;
using System.IO;
using System.Linq;
using Game.Base;
using Game.Base.Domain.Customers;
using Game.Services.Common;
using Game.Services.Events;
using Game.Services.Security;

namespace Game.Services.Customers
{
    /// <summary>
    /// Customer registration service
    /// </summary>
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Fields

        private const int SALT_KEY_SIZE = 5;

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        /// <param name="encryptionService">Encryption service</param>
        /// <param name="newsLetterSubscriptionService">Newsletter subscription service</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="storeService">Store service</param>
        /// <param name="rewardPointService">Reward points service</param>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="workContext">Work context</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="rewardPointsSettings">Reward points settings</param>
        /// <param name="customerSettings">Customer settings</param>
        public CustomerRegistrationService(ICustomerService customerService, 
            IEncryptionService encryptionService, 
            IWorkContext workContext,
            IEventPublisher eventPublisher,
            CustomerSettings customerSettings)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
            this._workContext = workContext;
            this._eventPublisher = eventPublisher;
            this._customerSettings = customerSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(CustomerPassword customerPassword, string enteredPassword)
        {
            if (customerPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (customerPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, _customerSettings.HashedPasswordFormat);
                    break;
            }

            if (customerPassword.Password == null)
                return false;

            return customerPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="username">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual CustomerLoginResults ValidateCustomer(string username, string password,out Customer outCustomer)
        {
            outCustomer = null;
            var customer = _customerService.GetCustomerByUsername(username);
            if (customer == null)
                customer = _customerService.GetCustomerByEmail(username);
            if(customer==null)
                customer = _customerService.GetCustomerByPhoneNumber(username);

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //only registered can login
            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;
            //check whether a customer is locked out
            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return CustomerLoginResults.LockedOut;

            if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), password))
            {
                //wrong password
                customer.FailedLoginAttempts++;
                if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                    customer.FailedLoginAttempts >= _customerSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_customerSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    customer.FailedLoginAttempts = 0;
                }
                _customerService.UpdateCustomer(customer);

                return CustomerLoginResults.WrongPassword;
            }

            //update login details
            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.RequireReLogin = false;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            _customerService.UpdateCustomer(customer);

            outCustomer = customer;
            return CustomerLoginResults.Successful;
        }

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Customer == null)
                throw new ArgumentException("Can't load current customer");

            var result = new CustomerRegistrationResult();
            if (request.Customer.IsSearchEngineAccount())
            {
                result.AddError("Search engine can't be registered");
                return result;
            }
            if (request.Customer.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }
            if (request.Customer.IsRegistered())
            {
                result.AddError("Current customer is already registered");
                return result;
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("Email地址不能为空");
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("无效的Email地址");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("密码不能为空");
                return result;
            }

            if (string.IsNullOrEmpty(request.Username))
            {
                result.AddError("用户名不能为空");
                return result;
            }


            //validate unique user
            if (_customerService.GetCustomerByEmail(request.Email) != null)
            {
                result.AddError("Email地址已经存在");
                return result;
            }

            if (_customerService.GetCustomerByUsername(request.Username) != null)
            {
                result.AddError("用户名已经存在");
                return result;
            }

            if (_customerService.GetCustomerByPhoneNumber(request.PhoneNumber) != null)
            {
                result.AddError("手机号码已经存在");
                return result;
            }


            //at this point request is valid
            request.Customer.Username = request.Username;
            request.Customer.Email = request.Email;

            var customerPassword = new CustomerPassword
            {
                Customer = request.Customer,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    customerPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(SALT_KEY_SIZE);
                        customerPassword.PasswordSalt = saltKey;
                        customerPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _customerSettings.HashedPasswordFormat);
                    }
                    break;
            }
            _customerService.InsertCustomerPassword(customerPassword);

            request.Customer.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            if (registeredRole == null)
                throw new GameException("'Registered' role could not be loaded");
            request.Customer.CustomerCustomerRoleMapping.Add(new CustomerCustomerRoleMapping(request.Customer.Id, registeredRole.Id));
            //remove from 'Guests' role
            var guestRole = request.Customer.CustomerCustomerRoleMapping.FirstOrDefault(cr => cr.CustomerRole.SystemName == SystemCustomerRoleNames.Guests);
            if (guestRole != null)
                request.Customer.CustomerCustomerRoleMapping.Remove(guestRole);

            _customerService.UpdateCustomer(request.Customer);

            //publish event
            _eventPublisher.Publish(new CustomerPasswordChangedEvent(customerPassword));

            return result;
        }
        
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("Email地址不能为空");
                return result;
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("密码不能为空");
                return result;
            }

            var customer = _customerService.GetCustomerByEmail(request.Email);
            if (customer == null)
            {
                result.AddError("此Email地址未在系统中注册，无法查询关联客户");
                return result;
            }

            if (request.ValidateRequest)
            {
                //request isn't valid
                if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), request.OldPassword))
                {
                    result.AddError("旧密码不正确");
                    return result;
                }
            }

            //check for duplicates
            if (_customerSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = _customerService.GetCustomerPasswords(customer.Id, passwordsToReturn: _customerSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError("此密码已经使用过了，不能再次使用");
                    return result;
                }
            }

            //at this point request is valid
            var customerPassword = new CustomerPassword
            {
                Customer = customer,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                        customerPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                        customerPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(SALT_KEY_SIZE);
                        customerPassword.PasswordSalt = saltKey;
                        customerPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _customerSettings.HashedPasswordFormat);
                    }
                    break;
            }
            _customerService.InsertCustomerPassword(customerPassword);

            //publish event
            _eventPublisher.Publish(new CustomerPasswordChangedEvent(customerPassword));

            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        public virtual void SetEmail(Customer customer, string newEmail, bool requireValidation)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (newEmail == null)
                throw new GameException("Email cannot be null");

            newEmail = newEmail.Trim();
            var oldEmail = customer.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new GameException("新的Email地址无效");

            if (newEmail.Length > 100)
                throw new GameException("Email地址太长");

            var customer2 = _customerService.GetCustomerByEmail(newEmail);
            if (customer2 != null && customer.Id != customer2.Id)
                throw new GameException("Email地址已经存在");

            if (requireValidation)
            {
                //re-validate email
                customer.EmailToRevalidate = newEmail;
                _customerService.UpdateCustomer(customer);
            }
            else
            {
                customer.Email = newEmail;
                _customerService.UpdateCustomer(customer);
            }
        }

        /// <summary>
        /// Sets a customer username
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newUsername">New Username</param>
        public virtual void SetUsername(Customer customer, string newUsername)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            
            newUsername = newUsername.Trim();

            if (newUsername.Length > 100)
                throw new GameException("用户名太长");

            var user2 = _customerService.GetCustomerByUsername(newUsername);
            if (user2 != null && customer.Id != user2.Id)
                throw new GameException("用户名已经存在");

            customer.Username = newUsername;
            _customerService.UpdateCustomer(customer);
        }

        

        #endregion
    }
}