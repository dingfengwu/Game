using Game.Facade.Mvc.Models;

namespace Game.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseGameModel
    {
        public string ImpersonatedCustomerName { get; set; }
        public bool IsCustomerImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}