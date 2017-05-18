﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Energy.Base
{
    /// <summary>
    /// Geographic coordinate system
    /// </summary>
    public class Geo
    {
        /// <summary>
        /// Location point
        /// </summary>
        public class Point
        {
            [XmlElement("X")]
            [DefaultValue(0)]
            public double Latitude;

            [XmlElement("Y")]
            [DefaultValue(0)]
            public double Longitude;

            [XmlElement("Z")]
            [DefaultValue(0)]
            public double Altitude;

            public double GetDistance(Point location)
            {
                return Geo.GetDistance(this, location);
            }

            public override string ToString()
            {
                return base.ToString();
            }

            /// <summary>
            /// Create point object from string constant
            /// </summary>
            /// <param name="value"></param>
            public static implicit operator Point(string value)
            {
                string pattern = Energy.Base.Pattern.LatitudeAndLongitude;
                Match m = Regex.Match(value, pattern, RegexOptions.IgnorePatternWhitespace);

                double latitude_degree = Energy.Base.Cast.StringToDouble(m.Groups["latitude_degree"].Value);
                double latitude_minute = Energy.Base.Cast.StringToDouble(m.Groups["latitude_minute"].Value);
                double latitude_second = Energy.Base.Cast.StringToDouble(m.Groups["latitude_second"].Value);
                string latitude_direction = m.Groups["latitude_direction"].Value;

                double longitude_degree = Energy.Base.Cast.StringToDouble(m.Groups["longitude_degree"].Value);
                double longitude_minute = Energy.Base.Cast.StringToDouble(m.Groups["longitude_minute"].Value);
                double longitude_second = Energy.Base.Cast.StringToDouble(m.Groups["longitude_second"].Value);
                string longitude_direction = m.Groups["longitude_direction"].Value;

                double latitude = latitude_degree + latitude_minute / 60 + latitude_second / 3600;
                double longitude = longitude_degree + longitude_minute / 60 + longitude_second / 3600;

                if (latitude_direction == "S" || latitude_direction == "s")
                    latitude *= -1;
                if (longitude_direction == "W" || longitude_direction == "w")
                    longitude *= -1;

                Point location = new Point() { Latitude = latitude, Longitude = longitude };

                return location;
            }

            /// <summary>
            /// Represent location coordinates as latitude/longitude string
            /// </summary>
            /// <returns></returns>
            public string ToLatLong()
            {
                string latitude = Energy.Base.Cast.DoubleToString(Latitude, 6, true, null);
                string longitude = Energy.Base.Cast.DoubleToString(Longitude, 6, true, null);
                return String.Concat("(", latitude, " , ", longitude, ")");
            }

            /// <summary>
            /// Represent loaction coordinates as degrees minutes seconds (DMS) string
            /// </summary>
            /// <returns></returns>
            public string ToDMS()
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                if (Latitude > 0)
                    list.Add(Energy.Base.Cast.DoubleToDMS(Latitude) + " N");
                if (Latitude < 0)
                    list.Add(Energy.Base.Cast.DoubleToDMS(-Latitude) + " S");
                if (Longitude > 0)
                    list.Add(Energy.Base.Cast.DoubleToDMS(Longitude) + " E");
                if (Longitude < 0)
                    list.Add(Energy.Base.Cast.DoubleToDMS(-Longitude) + " W");
                return String.Join(" ", list.ToArray());
            }
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="value">Degrees</param>
        /// <returns>Radians</returns>
        public static double Deg2Rad(double value)
        {
            return value * Math.PI / 180;
        }

        /// <summary>
        /// Earth radius
        /// </summary>
        public const double R = 6371;

        /// <summary>
        /// Get distance between two locations in kilometers
        /// </summary>
        /// <param name="location1">First location</param>
        /// <param name="location2">Second location</param>
        /// <returns>Distance</returns>
        public static double GetDistance(Point location1, Point location2)
        {
            var longitudeDifference = Deg2Rad(location2.Longitude - location1.Longitude);
            var latitudeDifference = Deg2Rad(location2.Latitude - location1.Latitude);

            var a = Math.Sin(latitudeDifference / 2) * Math.Sin(latitudeDifference / 2)
              + Math.Cos(Deg2Rad(location1.Latitude)) * Math.Cos(Deg2Rad(location2.Latitude))
              * Math.Sin(longitudeDifference / 2) * Math.Sin(longitudeDifference / 2)
              ;

            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
