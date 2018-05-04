using FluentValidation.Attributes;
using Game.Facade.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Models.Account
{
    [Validator(typeof(SignInPostModel))]
    public class SignInPostModel: BaseGameModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool RememberMe { get; set; }
    }
}
