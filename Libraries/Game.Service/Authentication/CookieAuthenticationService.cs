using Game.Base.Domain.Customers;
using Game.Services.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Game.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerSettings">Customer settings</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        public CookieAuthenticationService(CustomerSettings customerSettings,
            ICustomerService customerService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._customerSettings = customerSettings;
            this._customerService = customerService;
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        public virtual async void SignIn(Customer customer, bool isPersistent)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //create claims for customer's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(customer.Username))
                claims.Add(new Claim(ClaimTypes.Name, customer.Username, ClaimValueTypes.String, GameCookieAuthenticationDefaults.ClaimsIssuer));

            if (!string.IsNullOrEmpty(customer.Email))
                claims.Add(new Claim(ClaimTypes.Email, customer.Email, ClaimValueTypes.Email, GameCookieAuthenticationDefaults.ClaimsIssuer));

            if (!string.IsNullOrEmpty(customer.PhoneNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, customer.PhoneNumber, ClaimValueTypes.String, GameCookieAuthenticationDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, GameCookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(GameCookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated customer
            _cachedCustomer = customer;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual async void SignOut()
        {
            //reset cached customer
            _cachedCustomer = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(GameCookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual Customer GetAuthenticatedCustomer()
        {
            //whether there is a cached customer
            if (_cachedCustomer != null)
                return _cachedCustomer;

            //try to get authenticated user identity
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(GameCookieAuthenticationDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            Customer customer = null;
            if (customer == null)
            {
                //try to get customer by username
                var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                        && claim.Issuer.Equals(GameCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (usernameClaim != null)
                    customer = _customerService.GetCustomerByUsername(usernameClaim.Value);
            }
            
            if (customer == null)
            {
                //try to get customer by email
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(GameCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (emailClaim != null)
                    customer = _customerService.GetCustomerByEmail(emailClaim.Value);
            }

            if (customer == null)
            {
                //try to get customer by phone number
                var phoneNumberClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.MobilePhone
                    && claim.Issuer.Equals(GameCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (phoneNumberClaim != null)
                    customer = _customerService.GetCustomerByPhoneNumber(phoneNumberClaim.Value);
            }

            //whether the found customer is available
            if (customer == null || !customer.Active || customer.RequireReLogin || customer.Deleted || !customer.IsRegistered())
                return null;

            //cache authenticated customer
            _cachedCustomer = customer;

            return _cachedCustomer;
        }

        #endregion
    }
}