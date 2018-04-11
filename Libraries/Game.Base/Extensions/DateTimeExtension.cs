using Game.Base.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Extensions
{
    public static class DateTimeExtension
    {
        public static string FormatForDate(this DateTime @this,Language language)
        {
            if (language.LanguageCulture == "en-US")
            {
                return @this.ToString("MM/dd ddd");
            }
            else if (language.LanguageCulture == "zh-CN")
            {
                return @this.ToString("MM月dd日 ddd");
            }
            else
            {
                return @this.ToString("MM/dd ddd");
            }
        }
    }
}
