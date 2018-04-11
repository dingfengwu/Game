using Game.Facade.Mvc.Models;

namespace Game.Web.Models.Common
{
    public partial class LanguageModel : BaseGameEntityModel
    {
        public string Name { get; set; }

        public string FlagImageFileName { get; set; }
    }
}