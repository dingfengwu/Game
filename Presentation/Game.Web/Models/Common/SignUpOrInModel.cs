using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Models.Common
{
    public class SignUpOrInModel
    {
        public bool RegisteredUser { get; set; }

        public int UserId { get; set; }

        public string UserIcon { get; set; }

    }
}
