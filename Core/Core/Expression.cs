using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Core
{
    public class Expression
    {

        public sealed class Repository
        {
            private static readonly Repository instance = new Repository();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            static Repository()
            {
            }

            private Repository()
            {
            }

            public static Repository Instance
            {
                get
                {
                    return instance;
                }
            }

            public Dictionary<string, Regex> Dictionary = new Dictionary<string, Regex>();

            public static Regex Get(string pattern, RegexOptions option = RegexOptions.None)
            {
                if (option == RegexOptions.None)
                {
                    option = RegexOptions.IgnoreCase;
                }
                option |= RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace;
                foreach (KeyValuePair<string, Regex> item in Instance.Dictionary)
                {
                    if (item.Key == pattern && item.Value.Options == option)
                    {
                        return item.Value;
                    }
                }
                Regex expression = new Regex(pattern, option);
                Instance.Dictionary.Add(pattern, expression);
                return expression;
            }
        }
    }
}
