using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Regular expression pattern constants
    /// </summary>
    public class Pattern
    {        
        /// <summary>
        /// Connection string pattern
        /// </summary>
        public readonly static string ConnectionString = "(?<key>{[^}]*}|[^;=\\r\\n]+)(?:=(?<value>\"(?:\"\"|[^\"])*\"|{[^}]*}|[^;]*))?(?:;)?";

        /// <summary>
        /// Geolocation string containg longitude and latitude values.
        /// 
        /// Allows matching several standards:
        /// 66° 33′ 39″ N 0° 0′ 0″
        /// 51.5074° N, 0.1278° W
        /// 51,5074° N 0,1278° W
        /// 51,5074 N 0,1278 W
        /// -51,5074 -0,1278
        /// </summary>
        public readonly static string LatitudeAndLongitude = @"(?:
(?:
(?<latitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*°\s*
(?:(?<latitude_minute>[-+]?\d+(?:[.,]\d+)?)\s*[′']\s*)?
(?:(?<latitude_second>[-+]?\d+(?:[.,]\d+)?)\s*[″""]\s*)?
)
|
(?:
(?<latitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*
)
)
(?:(?<latitude_direction>[NSns])\s*?)
(?:[,.]\s*)?
(?:
(?:
(?<longitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*°\s*
(?:(?<longitude_minute>[-+]?\d+(?:[.,]\d+)?)\s*[′']\s*)?
(?:(?<longitude_second>[-+]?\d+(?:[.,]\d+)?)\s*[″""]\s*)?
)
|
(?:
(?<longitude_degree>[-+]?\d+(?:[.,]\d+)?)\s*
)
)
(?:(?<longitude_direction>[WEwe])\s*?)
";
    }
}
