using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Energy.Base
{
    /// <summary>
    /// Naming convention
    /// </summary>
    public static class Naming
    {
        #region CamelCase

        /// <summary>
        /// Return words capitalized with first word in lower case.
        /// <br/><br/>
        /// camelCase
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string CamelCase(string[] words)
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
                //words[0] = words[0].ToLowerInvariant();
                words[0] = words[0].ToLower(CultureInfo.InvariantCulture);
                for (int i = 1; i < words.Length; i++)
                {
                    words[i] = Energy.Base.Text.Capitalize(words[i]);
                }
                return string.Join("", words);
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in camelCase.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsCamelCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+)(?:\p{Lu}\p{Ll}*|\d+)*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region CobolCase

        /// <summary>
        /// Return words upper case, separated with hyphen character.
        /// <br/><br/>
        /// COBOL-CASE
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string CobolCase(string[] words)
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
                return string.Join("-", Energy.Base.Text.Upper(words));
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in COBOL-CASE.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsCobolCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Lu}+|\d+)(?:-(?:\p{Lu}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region ConstantCase

        /// <summary>
        /// Return words upper case, separated with underscore character.
        /// <br/><br/>
        /// CONSTANT_CASE
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string ConstantCase(string[] words)
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
                return string.Join("_", Energy.Base.Text.Upper(words));
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in CONSTANT_CASE.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsConstantCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Lu}+|\d+)(?:_(?:\p{Lu}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region DashCase

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// dash-case
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Also known as kebab-case or hyphen-case.
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
        /// Check if text is valid identifier written in dash-case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDashCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+|\d+)(?:-(?:\p{Ll}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region HyphenCase

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// hyphen-case
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Also known as dash-case or kebab-case.
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
        /// Check if text is valid identifier written in hyphen-case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsHyphenCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+|\d+)(?:-(?:\p{Ll}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region KebabCase

        /// <summary>
        /// Return words lower case, separated with hyphen character.
        /// <br/><br/>
        /// kebab-case
        /// <br/><br/>
        /// This style is often used in URLs to give more human-readable look.
        /// <br/><br/>
        /// Also known as dash-case or hyphen-case.
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
        /// Check if text is valid identifier written in kebab-case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsKebabCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+|\d+)(?:-(?:\p{Ll}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region PascalCase

        /// <summary>
        /// Return words capitalized.
        /// <br/><br/>
        /// PascalCase
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string PascalCase(string[] words)
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
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = Energy.Base.Text.Capitalize(words[i]);
                }
                return string.Join("", words);
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in PascalCase.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsPascalCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Lu}\p{Ll}*)(?:\p{Lu}\p{Ll}*|\d+)*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region SnakeCase

        /// <summary>
        /// Return words lower case, joined with underscore character.
        /// <br/><br/>
        /// snake_case
        /// <br/><br/>
        /// Also known as underscore_case.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string SnakeCase(string[] words)
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
                return string.Join("_", Energy.Base.Text.Lower(words));
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in snake_case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsSnakeCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+|\d+)(?:_(?:\p{Ll}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region TrainCase

        /// <summary>
        /// Return words capitalized and separated by hyphen character.
        /// <br/><br/>
        /// Train-Case
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string TrainCase(string[] words)
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
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = Energy.Base.Text.Capitalize(words[i]);
                }
                return string.Join("-", words);
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in Train-Case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTrainCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Lu}\p{Ll}*)(?:-(?:\p{Lu}\p{Ll}*|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion

        #region UnderscoreCase

        /// <summary>
        /// Return words lower case, joined with underscore character.
        /// <br/><br/>
        /// underscore_case
        /// <br/><br/>
        /// Also known as snake_case.
        /// </summary>
        /// <param name="words">Array of words</param>
        /// <returns>String</returns>
        public static string UnderscoreCase(string[] words)
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
                return string.Join("_", Energy.Base.Text.Lower(words));
            }
        }

        /// <summary>
        /// Check if text is valid identifier written in underscore_case.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsUnderscoreCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string pattern = @"^(?:\p{Ll}+)(?:_(?:\p{Ll}+|\d+))*$";
            return Regex.Match(text, pattern, RegexOptions.CultureInvariant).Success;
        }

        #endregion
    }
}
