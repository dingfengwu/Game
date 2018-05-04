using Game.Base.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Game.Base.Extensions
{
    public static class DateTimeExtension
    {
        public static string FormatForDate(this DateTime @this,Language language)
        {
            if (language.LanguageCulture == "en-US")
            {
                return @this.ToString("MM/dd ddd",CultureInfo.GetCultureInfo("en-US"));
            }
            else if (language.LanguageCulture == "zh-CN")
            {
                return @this.ToString("MM月dd日 ddd",CultureInfo.GetCultureInfo("zh-CN"));
            }
            else
            {
                return @this.ToString("MM/dd ddd");
            }
        }
    }
}
