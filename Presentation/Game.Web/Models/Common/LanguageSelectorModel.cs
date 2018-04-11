﻿using Game.Facade.Mvc.Models;
using System.Collections.Generic;

namespace Game.Web.Models.Common
{
    public partial class LanguageSelectorModel : BaseGameModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UseImages { get; set; }
    }
}