using Game.Facade.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Models.Account
{
    public class SignUpPostModel:BaseGameModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string RealName { get; set; }
    }
}
