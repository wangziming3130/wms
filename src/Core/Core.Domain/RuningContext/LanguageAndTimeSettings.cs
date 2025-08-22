using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public static class LanguageAndTimeSettings
    {
        public static string Language { get; set; }
        public static string TimeZone { get; set; }
        public static string DateFormat { get; set; }
        public static string TimeFormat { get; set; }
        public static string TimeFormatWithoutSecond { get; set; }
        public static string DateTimeFormatWithoutSecond => $"{DateFormat} {TimeFormatWithoutSecond}";

        public static CultureInfo GetCultureInfo()
        {
            if (string.IsNullOrWhiteSpace(Language))
            {
                return new CultureInfo(1033, true);
            }
            return (Language.ToLowerInvariant()) switch
            {
                "ja" => new CultureInfo(1041, true),
                "ja-jp" => new CultureInfo(1041, true),
                "en" => new CultureInfo(1033, true),
                "en-us" => new CultureInfo(1033, true),
                _ => new CultureInfo(Language, true),
            };
        }
    }
}
