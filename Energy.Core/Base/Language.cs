using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Language
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Language code specification according to ISO-639
        /// </summary>
        public class Code
        {
            /// <summary>
            /// ISO 639-1 two-letter language code
            /// </summary>
            public string Short { get; set; }

            /// <summary>
            /// ISO 639-2 three-letter language code
            /// </summary>
            public string Long { get; set; }

            /// <summary>
            /// ISO 639-2/T three-letter terminology (T) language code
            /// </summary>
            public string Terminology { get; set; }

            /// <summary>
            /// ISO 639-2/B three-letter bibliographic (B) language code
            /// </summary>
            public string Bibliographic { get; set; }

            /// <summary>
            /// ISO 639 language code with ISO 3166 country code
            /// </summary>
            public string Territory { get; set; }
        }

        /// <summary>
        /// Language native name
        /// </summary>
        public string NativeName { get; set; }

        /// <summary>
        /// Language english name
        /// </summary>
        public string EnglishName { get; set; }
    }
}
