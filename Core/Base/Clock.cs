using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Energy.Base
{
    /// <summary>
    /// Date and time
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Return current time in 24h/ms format, i.e. "12:33:15.176"
        /// </summary>
        public static string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss.fff");
            }
        }

        /// <summary>
        /// Return time as unix time
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public static double GetUnixTime(DateTime stamp)
        {
            TimeSpan span = (stamp - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return span.TotalSeconds;
        }

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
        /// Represent date and time as ISO readable format with zone setting, like "2016-03-02 12:00:01.340 +01:00". 
        /// If day is not set (equal to "0001-01-01", only time will be returned. Milliseconds are optional.
        /// </summary>
        /// <param name="stamp">Date and time</param>
        /// <returns>Date, time and zone ISO readable string</returns>
        public static string GetZoneString(DateTime? stamp)
        {
            if (stamp == null || stamp == DateTime.MinValue)
            {
                return String.Empty;
            }

            string format = "yyyy-MM-dd HH:mm:ss.fff zzz";

            if (((DateTime)stamp).Day == DateTime.MinValue.Day)
            {
                format = format.Replace("yyyy-MM-dd ", "");
            }

            if (((DateTime)stamp).Millisecond == 0)
            {
                format = format.Replace(".fff", "");
            }

            return ((DateTime)stamp).ToString(format);
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
    }
}
