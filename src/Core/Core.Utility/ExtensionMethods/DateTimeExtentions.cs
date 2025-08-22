using Core.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace Core.Utility
{
    public static class DateTimeExtentions
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);

        public static string FormatTimeWithoutSecond(this DateTime dataTime, string format = "", IFormatProvider provider = null, bool useCurrentCulture = true)
        {
            format = !string.IsNullOrEmpty(format) ? format
                : (!string.IsNullOrWhiteSpace(LanguageAndTimeSettings.TimeFormatWithoutSecond)
                    ? LanguageAndTimeSettings.TimeFormatWithoutSecond
                        : TimeFormat.DefaultWithoutSecound);
            return FormatDateInteral(dataTime, format, provider, useCurrentCulture);
        }
        public static string FormatDate(this DateTime dataTime, string format = "", IFormatProvider provider = null, bool useCurrentCulture = true)
        {
            format = !string.IsNullOrEmpty(format) ? format
               : (!string.IsNullOrWhiteSpace(LanguageAndTimeSettings.DateFormat)
                   ? LanguageAndTimeSettings.DateFormat
                       : DateTimeFormat.DefaultDate);
            return FormatDateInteral(dataTime, format, provider, useCurrentCulture);
        }

        public static string FormatDateTime(this DateTime dataTime, string format = "", IFormatProvider provider = null, bool useCurrentCulture = true)
        {
            format = !string.IsNullOrEmpty(format) ? format
               : (!string.IsNullOrWhiteSpace(LanguageAndTimeSettings.DateFormat)
               && !string.IsNullOrWhiteSpace(LanguageAndTimeSettings.TimeFormatWithoutSecond)
                   ? $"{LanguageAndTimeSettings.DateFormat} {LanguageAndTimeSettings.TimeFormatWithoutSecond}"
                       : DateTimeFormat.Default);
            return FormatDateInteral(dataTime, format, provider, useCurrentCulture);
        }


        private static string FormatDateInteral(DateTime dataTime, string format, IFormatProvider provider, bool useCurrentCulture)
        {
            return dataTime.ToString(format, LanguageAndTimeSettings.GetCultureInfo());
        }

        // dd/MM/yyyy HH:mm
        public static DateTime ConvertToDateTime(string date, string format = "")
        {
            format = string.IsNullOrEmpty(format) ? DateTimeFormat.Default : format;
            return DateTime.ParseExact(date, format, CultureInfo.CurrentCulture);
        }
        // dd/MM/yyyy
        public static DateTime ConvertToDatatimeUseDateString(string date, string format = "")
        {
            format = string.IsNullOrEmpty(format) ? DateTimeFormat.DefaultDate : format;
            return DateTime.ParseExact(date, format, CultureInfo.CurrentCulture);
        }

        public static bool TryToConvertDateTime(string data, out DateTime dateTime, string format)
        {
            return DateTime.TryParseExact(data, format, null, DateTimeStyles.None, out dateTime);
        }
    }



    public static class UnifiedTimezone
    {
        //long to DateTime
        public static DateTime ConvertToProfileDateTimeZone(long utcTicks)
        {
            return new DateTime(utcTicks, DateTimeKind.Utc);
        }
        //DateTime to long
        public static long ConvertToUTCTicks(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().Ticks;
        }
        //(UTC)long to Specified DateTime by TimeZoneInfo , TimeZone = Singapore Standard Time
        public static DateTime ConvertToSpecifiedTimeZone(long utcTicks)
        {
            DateTime targetDt = TimeZoneInfo.ConvertTimeFromUtc(ConvertTime(new DateTime(utcTicks, DateTimeKind.Utc)), GetProfileTimeZone());
            return targetDt;
        }

        //(UTC)DateTime to Specified DateTime by TimeZoneInfo , TimeZone = Singapore Standard Time
        public static DateTime ConvertToSpecifiedTimeZoneWithoutTimeZone(DateTime profileDateTime)
        {

            DateTime targetDt = TimeZoneInfo.ConvertTimeFromUtc(ConvertTime(profileDateTime), GetProfileTimeZone());
            return targetDt;
        }

        //(UTC)DateTime to Specified DateTime by TimeZoneInfo
        public static DateTime ConvertToSpecifiedTimeZone(DateTime utcDateTime, TimeZoneInfo timeZoneInfo)
        {
            DateTime targetDt = TimeZoneInfo.ConvertTimeFromUtc(ConvertTime(utcDateTime), timeZoneInfo);
            return targetDt;
        }
        //Specified DateTime to (UTC)DateTime by TimeZoneInfo , TimeZone = Singapore Standard Time
        public static DateTime ConvertToUtcTimeZone(DateTime profileDateTime)
        {
            DateTime targetDt = TimeZoneInfo.ConvertTimeToUtc(ConvertTime(profileDateTime), GetProfileTimeZone());
            return targetDt;
        }

        //Specified DateTime to (UTC)DateTime by TimeZoneInfo
        public static DateTime ConvertFromSpecifiedTimeZoneToUtc(DateTime utcDateTime, TimeZoneInfo timeZoneInfo)
        {
            DateTime targetDt = TimeZoneInfo.ConvertTimeToUtc(ConvertTime(utcDateTime), timeZoneInfo);
            return targetDt;
        }

        //(UTC)DateTime to Specified DateTime by minutesTimeZoneOffset
        public static DateTime ConvertToSpecifiedTimeZone(DateTime utcDateTime, double minutesTimeZoneOffset)
        {
            var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
            var timeSpan = TimeSpan.FromMinutes(minutesTimeZoneOffset);
            var timeZone = systemTimeZones.FirstOrDefault(tz => tz.BaseUtcOffset == timeSpan);

            DateTime targetDateTime = utcDateTime;
            if (timeZone != null)
            {
                targetDateTime = ConvertToSpecifiedTimeZone(ConvertTime(utcDateTime), timeZone);
            }
            return targetDateTime;
        }

        //Specified DateTime to(UTC)DateTime by minutesTimeZoneOffset
        public static DateTime ConvertFromSpecifiedTimeZoneToUtc(DateTime utcDateTime, double minutesTimeZoneOffset)
        {
            var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
            var timeSpan = TimeSpan.FromMinutes(minutesTimeZoneOffset);
            var timeZone = systemTimeZones.FirstOrDefault(tz => tz.BaseUtcOffset == timeSpan);

            DateTime targetDateTime = utcDateTime;
            if (timeZone != null)
            {
                targetDateTime = ConvertFromSpecifiedTimeZoneToUtc(ConvertTime(utcDateTime), timeZone);
            }
            return targetDateTime;
        }

        public static TimeZoneInfo GetProfileTimeZone()
        {
            var profileTimeZone = TZConvert.GetTimeZoneInfo(LanguageAndTimeSettings.TimeZone);
            if (profileTimeZone != null)
            {
                return profileTimeZone;
            }
            else
            {
                return RuningContextConstants.DefaultTimeZone;
            }
        }

        private static DateTime ConvertTime(DateTime time)
        {
            DateTime res;
            var time24 = time.ToString(DateTimeFormat.DefaultWithSecound);
            try
            {
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                dtFormat.ShortDatePattern = DateTimeFormat.DefaultWithSecound;

                res = Convert.ToDateTime(time24, dtFormat);
            }
            catch (Exception)
            {
                res = DateTime.ParseExact(time24, DateTimeFormat.DefaultWithSecound, CultureInfo.InvariantCulture);
            }
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("zh-SG");
            //return DateTime.Parse(time24);
            return res;
        }
    }

    public static class JsDateTimeHelper
    {
        /// <summary>
        /// C# ticks to js time
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static long ConvertTicksFromUtc(long ticks)
        {
            long origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            return (ticks - origin) / 10000;
        }

        /// <summary>
        /// js time to C# ticks
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static long ConvertTicksToUtc(long ticks)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return ticks * 10000 + origin.Ticks;
        }
    }

    public static class TimeFormat
    {
        public static readonly string Default = "HH:mm:ss";
        public static readonly string DefaultWithoutSecound = "HH:mm";
    }

    public static class DateTimeFormat
    {
        public static readonly string Default = "dd/MM/yyyy HH:mm";
        public static readonly string DefaultWithSecound = "dd/MM/yyyy HH:mm:ss";
        public static readonly string DefaultDate = "dd/MM/yyyy";
        // use for CM
        public static readonly string DateTimeFormatForCM = "%d/%M/yyyy, HH:mm";
    }
}
