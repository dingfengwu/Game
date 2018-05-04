using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Game.Base;
using Game.Base.Domain.Customers;
using Game.Facade;
using Game.Facade.Mvc.Filters;
using Game.Facade.Security;
using Game.Facade.Security.Captcha;
using Game.Services.Authentication;
using Game.Services.Common;
using Game.Services.Customers;
using Game.Services.Events;
using Game.Services.Localization;
using Game.Services.Logging;
using Game.Web.Controllers;
using Game.Web.Factories;
using Game.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class AccountController : BasePublicController
    {
        readonly IAuthenticationService _authenticationService;
        readonly ICustomerModelFactory _customerModelFactory;
        readonly CaptchaSettings _captchaSettings;
        readonly ILocalizationService _localizationService;
        readonly ICustomerRegistrationService _customerRegistrationService;
        readonly IEventPublisher _eventPublisher;
        readonly ICustomerActivityService _customerActivityService;
        readonly IWorkContext _workContext;
        readonly IGenericAttributeService _genericAttributeService;
        readonly CustomerSettings _customerSettings;
        readonly ICustomerService _customerService;
        readonly IWebHelper _webHelper;

        public AccountController(ICustomerModelFactory customerModelFactory,
            CaptchaSettings captchaSettings,
            ILocalizationService localizationService,
            ICustomerRegistrationService customerRegistrationService,
            IAuthenticationService authenticationService,
            IEventPublisher eventPublisher,
            ICustomerActivityService customerActivityService,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            CustomerSettings customerSettings,
            ICustomerService customerService,
            IWebHelper webHelper
            )
        {
            this._customerModelFactory = customerModelFactory;
            this._captchaSettings = captchaSettings;
            this._localizationService = localizationService;
            this._customerRegistrationService = customerRegistrationService;
            this._authenticationService = authenticationService;
            this._eventPublisher = eventPublisher;
            this._customerActivityService = customerActivityService;
            this._workContext = workContext;
            this._genericAttributeService = genericAttributeService;
            this._customerSettings = customerSettings;
            this._customerService = customerService;
            this._webHelper = webHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult SignIn()
        {
            var model = _customerModelFactory.PrepareLoginModel();
            model.DisplayCaptcha = true;
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        [ValidateCaptcha]
        public IActionResult SignIn(SignInPostModel model, string returnUrl, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                Customer customer = null;
                var loginResult = _customerRegistrationService.ValidateCustomer(model.Username, model.Password, out customer);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            //sign in new customer
                            _authenticationService.SignIn(customer, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            //activity log
                            _customerActivityService.InsertActivity(customer, "PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"));

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToAction("Index", "Profile", new { area = "" });

                            return Redirect(returnUrl);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }


            var returnModel = _customerModelFactory.PrepareLoginModel();
            returnModel.Username = model.Username;
            returnModel.RememberMe = model.RememberMe;
            return View(model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult SignUp()
        {
            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            var model = new SignUpPostModel();
            model = _customerModelFactory.PrepareRegisterModel(model, false, setDefaultValues: true);

            return View();
        }


        [HttpPost]
        [ValidateCaptcha]
        [ValidateHoneypot]
        [PublicAntiForgery]
        public IActionResult SignUp(SignUpPostModel model, string returnUrl, bool captchaValid)
        {
            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_workContext.CurrentCustomer.IsRegistered())
            {
                //Already registered customer. 
                _authenticationService.SignOut();

                //raise logged out event       
                _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

                //Save a new record
                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
            }
            var customer = _workContext.CurrentCustomer;

            //custom customer attributes
            //var customerAttributesXml = ParseCustomCustomerAttributes(model.Form);
            //var customerAttributeWarnings = _customerAttributeParser.GetAttributeWarnings(customerAttributesXml);
            //foreach (var error in customerAttributeWarnings)
            //{
            //    ModelState.AddModelError("", error);
            //}

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest(customer,
                    model.Email,
                    model.Username,
                    model.Password,
                    _customerSettings.DefaultPasswordFormat,
                    isApproved);
                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                if (registrationResult.Success)
                {
                    //login customer now
                    if (isApproved)
                        _authenticationService.SignIn(customer, true);
                    
                    //raise event       
                    _eventPublisher.Publish(new CustomerRegisteredEvent(customer));

                    switch (_customerSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            {
                                //email validation message
                                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
                                //_workflowMessageService.SendCustomerEmailValidationMessage(customer, _workContext.WorkingLanguage.Id);

                                //result
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.EmailValidation});
                            }
                        case UserRegistrationType.AdminApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdminApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //send customer welcome message
                                //_workflowMessageService.SendCustomerWelcomeMessage(customer, _workContext.WorkingLanguage.Id);

                                var redirectUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard});
                                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                    redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl=" + WebUtility.UrlEncode(returnUrl), null);
                                return Redirect(redirectUrl);
                            }
                        default:
                            {
                                return RedirectToRoute("HomePage");
                            }
                    }
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            model = _customerModelFactory.PrepareRegisterModel(model, true, setDefaultValues: true);
            return View(model);
        }

        public IActionResult SignOut()
        {
            if (_workContext.OriginalCustomerIfImpersonated != null)
            {
                //activity log
                _customerActivityService.InsertActivity(_workContext.OriginalCustomerIfImpersonated,
                    "Impersonation.Finished",
                    _localizationService.GetResource("ActivityLog.Impersonation.Finished.StoreOwner"),
                    _workContext.CurrentCustomer.Email, _workContext.CurrentCustomer.Id);
                _customerActivityService.InsertActivity("Impersonation.Finished",
                    _localizationService.GetResource("ActivityLog.Impersonation.Finished.Customer"),
                    _workContext.OriginalCustomerIfImpersonated.Email, _workContext.OriginalCustomerIfImpersonated.Id);

                //logout impersonated customer
                _genericAttributeService.SaveAttribute<int?>(_workContext.OriginalCustomerIfImpersonated,
                    SystemCustomerAttributeNames.ImpersonatedCustomerId, null);

                //redirect back to customer details page (admin area)
                return RedirectToRoute("HomePage");
            }

            //activity log
            _customerActivityService.InsertActivity("PublicStore.Logout", _localizationService.GetResource("ActivityLog.PublicStore.Logout"));

            //standard logout 
            _authenticationService.SignOut();

            //raise logged out event       
            _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));
            
            return RedirectToRoute("HomePage");
        }

        public virtual IActionResult RegisterResult(int resultId)
        {
            //TODO:应该显示欢迎页，或进行激活的页面

            return RedirectToAction("Index", "Profile");

            var model = _customerModelFactory.PrepareRegisterResultModel(resultId);
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult RegisterResult(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToRoute("HomePage");

            return Redirect(returnUrl);
        }
    }
}