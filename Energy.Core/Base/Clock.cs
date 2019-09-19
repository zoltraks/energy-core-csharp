using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Energy.Base
{
    /// <summary>
    /// Date and time
    /// </summary>
    public static class Clock
    {
        #region Constant

        public static readonly TimeSpan Midday = TimeSpan.FromHours(12);

        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region CurrentTime

        /// <summary>
        /// Return current time in 24h/ms format, i.e. "17:33:15.176"
        /// </summary>
        public static string CurrentTime
        {
            get
            {
                return CurrentTimeMilliseconds;
            }
        }

        /// <summary>
        /// Return current time in 24h/ms format with trailing space, i.e. "17:33:15.176 "
        /// </summary>
        public static string CurrentTimeSpace
        {
            get
            {
                return CurrentTimeMilliseconds + " ";
            }
        }

        /// <summary>
        /// Return current time in 24h/ms format, i.e. "17:33"
        /// </summary>
        public static string CurrentTimeShort
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        /// <summary>
        /// Return current time in 24h ms format, i.e. "17:33:15.123"
        /// </summary>
        public static string CurrentTimeMilliseconds
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss.fff");
            }
        }

        /// <summary>
        /// Return current time in 24h μs format, i.e. "17:33:15.123456"
        /// </summary>
        public static string CurrentTimeMicroseconds
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss.ffffff");
            }
        }

        /// <summary>
        /// Return current time in 24h format, i.e. "17:33:15"
        /// </summary>
        public static string CurrentTimeSeconds
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        /// <summary>
        /// Return current time in 24h format, i.e. "17:33"
        /// </summary>
        public static string CurrentTimeMinutes
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        /// <summary>
        /// Return current time in 24h/ms format, i.e. "17:33:15.176"
        /// </summary>
        public static string CurrentUtcTime
        {
            get
            {
                return CurrentUtcTimeMilliseconds;
            }
        }

        /// <summary>
        /// Return current time in short 24h format, i.e. "17:33"
        /// </summary>
        public static string CurrentUtcTimeShort
        {
            get
            {
                return DateTime.UtcNow.ToString("HH:mm:ss");
            }
        }

        /// <summary>
        /// Return current time in 24h ms format, i.e. "17:33:15.123"
        /// </summary>
        public static string CurrentUtcTimeMilliseconds
        {
            get
            {
                return DateTime.UtcNow.ToString("HH:mm:ss.fff");
            }
        }

        /// <summary>
        /// Return current time in 24h μs format, i.e. "17:33:15.123456"
        /// </summary>
        public static string CurrentUtcTimeMicroseconds
        {
            get
            {
                return DateTime.UtcNow.ToString("HH:mm:ss.ffffff");
            }
        }

        /// <summary>
        /// Return current time in unix time format, i.e. 1461477755.353
        /// </summary>
        /// <returns></returns>
        public static double CurrentUnixTime
        {
            get
            {
                return GetUnixTime(DateTime.Now);
            }
        }

        #endregion

        #region Unix

        /// <summary>
        /// Return time as unix time
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public static double GetUnixTime(DateTime stamp)
        {
            TimeSpan span = (stamp.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return span.TotalSeconds;
        }

        /// <summary>
        /// Return unix time as DateTime
        /// </summary>
        /// <param name="unix">Unix time</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTime(double unix)
        {
            return UnixEpoch.AddSeconds(unix).ToLocalTime();
        }

        /// <summary>
        /// Return time as unix time
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="yearZero">Year of zero value, 1970 for unix</param>
        /// <returns></returns>
        public static double GetUnixTime(DateTime stamp, int yearZero)
        {
            TimeSpan span = (stamp.ToUniversalTime() - new DateTime(yearZero, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return span.TotalSeconds;
        }

        /// <summary>
        /// Return unix time as DateTime
        /// </summary>
        /// <param name="unix">Unix time</param>
        /// <param name="yearZero">Year of zero value, 1970 for unix</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTime(double unix, int yearZero)
        {
            DateTime epoch = new DateTime(yearZero, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unix).ToLocalTime();
        }

        #endregion

        #region ISO 8601

        /// <summary>
        /// Represent DateTime as simplified ISO date and time string, i.e. "2016-02-23 22:34:10"
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date and time string, time string if date was not specified, empty if null or equal to DateTime.MinValue</returns>
        public static string GetStampString(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            if (((DateTime)stamp).Date == DateTime.MinValue)
                return GetTimeString(stamp);
            if (((DateTime)stamp).Millisecond != 0)
                return ((DateTime)stamp).ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return ((DateTime)stamp).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO date string, i.e. "2016-02-23"
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetDateString(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            else
                return ((DateTime)stamp).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO time string, i.e. "22:39:07.350"
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetTimeString(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            if (((DateTime)stamp).Millisecond != 0)
                return ((DateTime)stamp).ToString("HH:mm:ss.fff");
            else
                return ((DateTime)stamp).ToString("HH:mm:ss");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO time string, i.e. "22:39:07.350"
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetTimeStringSeconds(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            return ((DateTime)stamp).ToString("HH:mm:ss");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO time string with milliseconds, i.e. "22:39:07.503".
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetTimeStringMilliseconds(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            return ((DateTime)stamp).ToString("HH:mm:ss.fff");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO time string with microseconds, i.e. "22:39:07.503726".
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetTimeStringMicroseconds(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            return ((DateTime)stamp).ToString("HH:mm:ss.ffffff");
        }

        /// <summary>
        /// Represent DateTime as simplified ISO time string with machine ticks (1/10 μs), i.e. "22:39:07.5039217".
        /// </summary>
        /// <param name="stamp">DateTime?</param>
        /// <returns>Date string, empty if null or equal to DateTime.MinValue</returns>
        public static string GetTimeStringTicks(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
                return String.Empty;
            return ((DateTime)stamp).ToString("HH:mm:ss.fffffff");
        }

        /// <summary>
        /// Represent TimeSpan as simplified ISO time string with total hours, like "01:07.350", or "123:01:02"
        /// </summary>
        /// <param name="span">TimeSpan?</param>
        /// <returns>Time string, empty if null or equal to TimeSpan.MinValue</returns>
        public static string GetTimeString(TimeSpan? span)
        {
            if (span == null || span == TimeSpan.MinValue)
                return String.Empty;
            System.Text.StringBuilder s = new System.Text.StringBuilder(12);
            if ((int)((TimeSpan)span).TotalHours > 0)
            {
                s.Append(((TimeSpan)span).TotalHours.ToString("00"));
                s.Append(':');
            }
            if ((int)((TimeSpan)span).TotalMinutes > 0)
            {
                s.Append(((TimeSpan)span).Minutes.ToString("00"));
                s.Append(':');
            }
            if (((TimeSpan)span).TotalSeconds > 10)
            {
                s.Append(((TimeSpan)span).Seconds.ToString("00"));
            }
            else
            {
                s.Append(((TimeSpan)span).Seconds.ToString());
            }
            if (((TimeSpan)span).TotalHours < 1)
            {
                if (((TimeSpan)span).Milliseconds != 0)
                {
                    s.Append('.');
                    s.Append(((TimeSpan)span).Milliseconds.ToString("000"));
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// Represent date and time strictly according to ISO 8601 standard
        /// with "T" for time, "Z" for UTC and "+/-" for time zone.
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public static string GetISO8601(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
            {
                return "";
            }

            int ms = ((DateTime)stamp).Millisecond;
            bool z = ((DateTime)stamp).Kind == DateTimeKind.Utc;
            string format = "";
            if (z)
            {
                format = ms > 0
                    ? "yyyy-MM-ddTHH:mm:ss.fffZ"
                    : "yyyy-MM-ddTHH:mm:ss.fffZ";
            }
            else
            {
                format = ms > 0
                    ? "yyyy-MM-ddTHH:mm:ss.fffzzz"
                    : "yyyy-MM-ddTHH:mm:sszzz";
            }

            return ((DateTime)stamp).ToString(format);
        }

        /// <summary>
        /// Represent date and time as ISO 8601 readable format with zone setting, like "2016-03-02 12:00:01.432 +01:00".
        /// On minimum value or null empty string will be returned.
        /// If day is equal to "0001-01-01", only time will be returned. Fractional part of second is optional.
        /// If precision value is negative, fractional part will always be present.
        /// </summary>
        /// <param name="stamp">Date and time</param>
        /// <param name="precision">Fractional part precision</param>
        /// <returns>Date, time and zone ISO readable string</returns>
        public static string GetZoneString(DateTime? stamp, int precision)
        {
            if (stamp == null || stamp == DateTime.MinValue)
            {
                return String.Empty;
            }

            DateTime value = (DateTime)stamp;

            string date = "";
            string time = "HH:mm:ss";
            string fraction = "";
            string zone = " zzz";

            if (precision < 0 || Energy.Base.Clock.HasFractionalPart(value))
            {
                if (precision < 0)
                {
                    precision = -precision;
                }

                if (precision > 7)
                {
                    precision = 7; // added to prevent exception "Input string was not in a correct format"
                }

                if (precision > 0)
                {
                    fraction = "." + new string('f', precision);
                }
            }

            if (value.Date != DateTime.MinValue.Date)
            {
                date = "yyyy-MM-dd ";
            }

            string format = date + time + fraction + zone;

            return value.ToString(format);
        }

        /// <summary>
        /// Represent date and time as ISO 8601 readable format with zone setting, like "2016-03-02 12:00:01.340 +01:00".
        /// On minimum value or null empty string will be returned.
        /// If day is equal to "0001-01-01", only time will be returned. Milliseconds are optionally added if present.
        /// </summary>
        /// <param name="stamp">Date and time</param>
        /// <returns>Date, time and zone ISO readable string</returns>
        public static string GetZoneString(DateTime? stamp)
        {
            return GetZoneString(stamp, 3);
        }

        #endregion

        #region Duration

        public class Duration : IXmlSerializable
        {
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                throw new NotImplementedException();
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Interval

        public class Interval : IXmlSerializable
        {
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                throw new NotImplementedException();
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Pattern

        private static readonly object _RegexLock = new object();

        private static Regex _DateRegex;
        /// <summary>Singleton</summary>
        public static Regex DateRegex
        {
            get
            {
                if (_DateRegex == null)
                {
                    lock (_RegexLock)
                    {
                        if (_DateRegex == null)
                        {
                            _DateRegex = new Regex(Energy.Base.Expression.Date, RegexOptions.Compiled);
                        }
                    }
                }
                return _DateRegex;
            }
        }

        private static Regex _TimeRegex;
        /// <summary>Singleton</summary>
        public static Regex TimeRegex
        {
            get
            {
                if (_TimeRegex == null)
                {
                    lock (_RegexLock)
                    {
                        if (_TimeRegex == null)
                        {
                            _TimeRegex = new Regex(Energy.Base.Expression.Time, RegexOptions.Compiled);
                        }
                    }
                }
                return _TimeRegex;
            }
        }

        #endregion

        #region Parse

        /// <summary>
        /// Parse stamp string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime Parse(string text)
        {
            DateTime result = DateTime.MinValue;

            if (String.IsNullOrEmpty(text))
                return result;

            text = text.Trim();
            int n;
            Match match;

            //match = Regex.Match(text, Energy.Base.Pattern.Date);
            match = DateRegex.Match(text);

            if (match.Success)
            {
                if (int.TryParse(match.Groups["year"].ToString(), out n))
                {
                    result = result.AddYears(n - 1);
                }
                if (int.TryParse(match.Groups["month"].ToString(), out n))
                {
                    result = result.AddMonths(n - 1);
                }
                if (int.TryParse(match.Groups["day"].ToString(), out n))
                {
                    result = result.AddDays(n - 1);
                }
            }

            //match = Regex.Match(text, Energy.Base.Pattern.Time);
            match = TimeRegex.Match(text);

            if (match.Success)
            {
                if (int.TryParse(match.Groups["hour"].ToString(), out n))
                {
                    result = result.AddHours(n);
                }
                if (int.TryParse(match.Groups["minute"].ToString(), out n))
                {
                    result = result.AddMinutes(n);
                }
                if (int.TryParse(match.Groups["second"].ToString(), out n))
                {
                    result = result.AddSeconds(n);
                }
                if (!string.IsNullOrEmpty(match.Groups["fraction"].Value))
                {
                    double d;
                    if (double.TryParse(string.Concat("0.", match.Groups["fraction"])
                        , System.Globalization.NumberStyles.AllowDecimalPoint
                        , System.Globalization.CultureInfo.InvariantCulture, out d))
                    {
                        result = result.AddSeconds(d);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Leap

        /// <summary>
        /// Sample subroutine to calculate if a year is a leap year.
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>True if a year is a leap year</returns>
        public static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        }

        #endregion

        #region Truncate

        /// <summary>
        /// Truncate DateTime to desired decimal precision of seconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static DateTime Truncate(DateTime dateTime, int precision)
        {
            TimeSpan time = Truncate(dateTime.TimeOfDay, precision);
            return dateTime.Date.Add(time);
        }

        /// <summary>
        /// Truncate DateTime to whole seconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Truncate(DateTime dateTime)
        {
            return Truncate(dateTime, 0);
        }

        /// <summary>
        /// Truncate TimeSpan to desired decimal precision of seconds
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static TimeSpan Truncate(TimeSpan timeSpan, int precision)
        {
            if (precision >= 7)
                return timeSpan;
            long ticks = timeSpan.Ticks;
            long seconds = ticks / System.TimeSpan.TicksPerSecond;
            if (precision < 1)
                return new TimeSpan(seconds * System.TimeSpan.TicksPerSecond);
            int fraction = (int)(ticks % System.TimeSpan.TicksPerSecond);
            int factor = (int)Energy.Base.Number.Power10[7 - precision];
            int reminder = factor * (int)(fraction / factor);
            return new TimeSpan(ticks - fraction + reminder);
        }

        #endregion

        #region Round

        /// <summary>
        /// Round DateTime to desired decimal precision of seconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static DateTime Round(DateTime dateTime, int precision)
        {
            TimeSpan time = Round(dateTime.TimeOfDay, precision);
            return dateTime.Date.Add(time);
        }

        /// <summary>
        /// Round DateTime to whole seconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Round(DateTime dateTime)
        {
            return Round(dateTime, 0);
        }

        /// <summary>
        /// Round TimeSpan to desired decimal precision of seconds
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static TimeSpan Round(TimeSpan timeSpan, int precision)
        {
            if (precision >= 7)
                return timeSpan;
            long ticks = timeSpan.Ticks;
            double seconds = 1.0 * ticks / System.TimeSpan.TicksPerSecond;
            double round = Math.Round(seconds, precision);
            return new TimeSpan((long)(round * System.TimeSpan.TicksPerSecond));
        }

        #endregion

        #region SetDate

        public static DateTime? SetDate(DateTime? value, int year, int month, int day)
        {
            if (value == null)
                return null;
            return new DateTime(year, month, day
                , ((DateTime)value).Hour, ((DateTime)value).Minute, ((DateTime)value).Second, ((DateTime)value).Millisecond
                , ((DateTime)value).Kind
                );
        }

        public static DateTime SetDate(DateTime value, int year, int month, int day)
        {
            return new DateTime(year, month, day
                , ((DateTime)value).Hour, ((DateTime)value).Minute, ((DateTime)value).Second, ((DateTime)value).Millisecond
                , ((DateTime)value).Kind
                );
        }

        #endregion

        #region HasExpired

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <param name="next">New check time</param>
        /// <param name="now">Moment of time to check for expiration</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime? last, double time, out DateTime? next, DateTime? now)
        {
            next = last;

            if (time < 0)
            {
                return false;
            }
            if (null == now || now == DateTime.MinValue)
            {
                now = DateTime.Now;
            }
            if (0 == time || null == last || last == DateTime.MinValue)
            {
                next = now;
                return true;
            }
            TimeSpan span = (DateTime)last - (DateTime)now;
            if (span.TotalSeconds < time)
            {
                return false;
            }
            else
            {
                next = now;
                return true;
            }
        }

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <param name="next">New check time</param>
        /// <param name="now">Moment of time to check for expiration</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime last, double time, out DateTime next, DateTime now)
        {
            next = last;

            if (time < 0)
            {
                return false;
            }
            if (null == now || now == DateTime.MinValue)
            {
                now = DateTime.Now;
            }
            if (0 == time || null == last || last == DateTime.MinValue)
            {
                next = now;
                return true;
            }
            TimeSpan span = last - now;
            if (span.TotalSeconds < time)
            {
                return false;
            }
            else
            {
                next = now;
                return true;
            }
        }

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <param name="next">New check time</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime? last, double time, out DateTime? next)
        {
            return HasExpired(last, time, out next, null);
        }

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime? last, double time)
        {
            DateTime? useless;
            return HasExpired(last, time, out useless);
        }

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <param name="next">New check time</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime last, double time, out DateTime next)
        {
            return HasExpired(last, time, out next, DateTime.MinValue);
        }

        /// <summary>
        /// Check if the time has expired.
        /// </summary>
        /// <param name="last">Last check time</param>
        /// <param name="time">Time limit in seconds</param>
        /// <returns></returns>
        public static bool HasExpired(DateTime last, double time)
        {
            DateTime useless;
            return HasExpired(last, time, out useless);
        }

        #endregion

        #region HasFractionalPart

        /// <summary>
        /// Check if DateTime value contains fractional part of seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasFractionalPart(DateTime value)
        {
            return 0 != value.Ticks % TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Check if TimeSpan value contains fractional part of seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasFractionalPart(TimeSpan value)
        {
            return 0 != value.Ticks % TimeSpan.TicksPerSecond;
        }

        #endregion

        /// <summary>
        /// Check if string is valid date and time string.
        /// Examples for positive match: "2019-01-20T00:00:01.345Z", " 2019-01-20 T 00:00:00.123456 Z + 03:30 ".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidDateTimeString(string input)
        {
            string pattern = @"(?<date>(\d{4}-\d{2}-\d{2}))\s*[Tt]?\s*(?<time>(\d{2}:\d{2}(:\d{2})?(.\d+)?))\s*[Zz]?\s*(?<zone>[+-]\s*\d{1,2}(:\d{2})?)?";
            return Regex.Match(input, pattern).Success;
        }

        public static bool IsValidDateString(string text, bool allowTime)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidTimeString(string text, bool allowDate)
        {
            throw new NotImplementedException();
        }

        #region Floor

        public static DateTime Floor(DateTime value, int precision)
        {
            if (DateTime.MinValue == value)
            {
                return DateTime.MinValue;
            }
            if (precision == 0)
            {
                return new DateTime(value.Year, value.Month, value.Day
                    , value.Hour, value.Minute, value.Second);
            }
            if (precision > 0)
            {
                if (precision > 7)
                {
                    precision = 7;
                }
                string format;
                if (precision == 1)
                {
                    format = "ff";
                }
                else
                {
                    format = new string('f', precision);
                }
                string text = value.ToString(format);
                if (precision == 1)
                {
                    text = text.Substring(0, 1);
                }
                double fraction = int.Parse(text);
                //for (int i = 0; i < precision; i++)
                //{
                //    fraction /= 10.0;
                //} // value will not have correct representation "0.4560000...7" instead of just "0.456"
                fraction /= Math.Pow(10, precision); // correct option
                DateTime result = new DateTime(value.Year, value.Month, value.Day
                    , value.Hour, value.Minute, value.Second);
                //result = result.AddTicks((long)fraction * TimeSpan.TicksPerSecond); // wrong version, will cast fraction to long first
                result = result.AddTicks((long)(fraction * TimeSpan.TicksPerSecond)); // correct version
                //result = result.AddTicks((long)TimeSpan.TicksPerSecond * fraction); // working version but still casting to longcorrect version
                //
                // fraction
                // 0.456
                // (long)fraction* TimeSpan.TicksPerSecond
                // 0
                // fraction* TimeSpan.TicksPerSecond
                // 4560000
                // (long) TimeSpan.TicksPerSecond* fraction
                // 4560000
                // (long)(fraction * TimeSpan.TicksPerSecond)
                // 4560000
                //
                return result;
            }
            if (precision < 0)
            {
                int year = value.Year;
                int month = value.Month;
                int day = value.Day;
                int hour = value.Hour;
                int minute = value.Minute;
                int second = value.Second;
                switch (precision)
                {
                    case 0:
                        break;
                    case -1:
                        second = second - (second % 10);
                        break;
                    case -2:
                        second = 0;
                        break;
                    case -3:
                        minute = minute - (minute % 10);
                        goto case -2;
                    case -4:
                        minute = 0;
                        goto case -2;
                    case -5:
                        hour = hour - (hour % 10);
                        goto case -4;
                    case -6:
                        hour = 0;
                        goto case -4;
                    case -7:
                        day = day - (day % 10);
                        goto case -6;
                    case -8:
                        day = 1;
                        goto case -6;
                    case -9:
                        month = month - (month % 10);
                        goto case -8;
                    case -10:
                        month = 1;
                        goto case -8;
                    case -11:
                        year = year - (year % 10);
                        goto case -10;
                    case -12:
                        year = year - (year % 100);
                        goto case -10;
                    case -13:
                        year = year - (year % 1000);
                        goto case -10;
                    default:
                    case -14:
                        year = 1;
                        goto case -10;
                }
                return new DateTime(year, month, day, hour, minute, second);
            }
            return DateTime.MinValue;
        }

        #endregion
    }
}
