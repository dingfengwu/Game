using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base.Domain.Customers;
using Game.Facade.Security.Captcha;
using Game.Services.Helpers;
using Game.Services.Localization;
using Game.Web.Models.Account;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Game.Web.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        readonly CaptchaSettings _captchaSettings;
        readonly DateTimeSettings _dateTimeSettings;
        readonly ILocalizationService _localizationService;

        public CustomerModelFactory(CaptchaSettings captchaSettings,
            DateTimeSettings dateTimeSettings,
            ILocalizationService localizationService)
        {
            this._captchaSettings = captchaSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._localizationService = localizationService;
        }

        public virtual SignInPostModel PrepareLoginModel()
        {
            var model = new SignInPostModel
            {
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage
            };
            return model;
        }

        public SignUpPostModel PrepareRegisterModel(SignUpPostModel model, bool excludeProperties, string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            
            return model;
        }

        /// <summary>
        /// Prepare the register result model
        /// </summary>
        /// <param name="resultId">Value of UserRegistrationType enum</param>
        /// <returns>Register result model</returns>
        public virtual RegisterResultModel PrepareRegisterResultModel(int resultId)
        {
            var resultText = "";
            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    resultText = _localizationService.GetResource("Account.Register.Result.Disabled");
                    break;
                case UserRegistrationType.Standard:
                    resultText = _localizationService.GetResource("Account.Register.Result.Standard");
                    break;
                case UserRegistrationType.AdminApproval:
                    resultText = _localizationService.GetResource("Account.Register.Result.AdminApproval");
                    break;
                case UserRegistrationType.EmailValidation:
                    resultText = _localizationService.GetResource("Account.Register.Result.EmailValidation");
                    break;
                default:
                    break;
            }
            var model = new RegisterResultModel
            {
                Result = resultText
            };
            return model;
        }
    }
}
