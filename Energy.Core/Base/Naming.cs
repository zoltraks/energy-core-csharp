using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Energy.Base
{
    /// <summary>
    /// Naming convention
    /// </summary>
    public static class Naming
    {
        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Known as dash-case, kebab-case or hyphen-case.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string DashCase(string[] words)
        {
            if (null == words)
            {
                return null;
            }
            else if (0 == words.Length)
            {
                return "";
            }
            else
            {
                return string.Join("-", Energy.Base.Text.Lower(words));
            }
        }

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Known as dash-case, kebab-case or hyphen-case.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string KebabCase(string[] words)
        {
            if (null == words)
            {
                return null;
            }
            else if (0 == words.Length)
            {
                return "";
            }
            else
            {
                return string.Join("-", Energy.Base.Text.Lower(words));
            }
        }

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Known as dash-case, kebab-case or hyphen-case.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string HyphenCase(string[] words)
        {
            if (null == words)
            {
                return null;
            }
            else if (0 == words.Length)
            {
                return "";
            }
            else
            {
                return string.Join("-", Energy.Base.Text.Lower(words));
            }
        }

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string SnakeCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            return string.Join("-", Energy.Base.Text.Lower(words));
        }

        /// <summary>
        /// Return words as a string separated with underscore character
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string UnderscoreCase(string[] words)
        {
            return string.Join("_", words);
        }

        /// <summary>
        /// Return words using medial capitalization
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string PascalCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = Energy.Base.Text.Capitalize(words[i]);
            }
            return string.Join("", words);
        }

        /// <summary>
        /// Return words using medial capitalization in optional java
        /// style of camel case
        /// </summary>
        /// <remarks>
        /// Example of getting "camelGoesFirst" word
        /// <pre>
        ///     string[] words = { "Camel", "gOES", "fIrSt" };
        ///     MessageBox.Show(Core.Transform.CamelCase(words, true));
        /// </pre>
        /// </remarks>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string CamelCase(string[] words)
        {
            if (words == null || words.Length == 0)
                return "";
            words[0] = words[0].ToLowerInvariant();
            for (int i = 1; i < words.Length; i++)
            {
                words[i] = Energy.Base.Text.Capitalize(words[i]);
            }
            return string.Join("", words);
        }

        /*

        /// <summary>
        /// Convert PascalCase to dash-case
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string PascalCaseToDashCase(string name)
        {
            string pattern = @"[A-Z]{2,}|[A-Z][^A-Z\s]*";
            MatchCollection matches = Regex.Matches(name, pattern);
            List<string> pieces = new List<string>();
            foreach (Match match in matches)
            {
                string value = match.Value;
                if (string.IsNullOrEmpty(value))
                    continue;
                value = value.Trim();
                if (value == "")
                    continue;
                pieces.Add(value);
            }
            string text = string.Join("-", pieces.ToArray());
            text = text.ToLowerInvariant();
            return text;
        }

        */
    }
}
