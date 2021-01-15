using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Energy.Base
{
    /// <summary>
    /// INI support
    /// </summary>
    public class Ini
    {
        [Serializable]
        public class IniFile : Dictionary<string, Energy.Base.Collection.StringDictionary>
        {
            private bool _CaseSensitive = true;

            public bool CaseSensitive
            {
                get
                {
                    return _CaseSensitive;
                }
                set
                {
                    if (_CaseSensitive == value)
                        return;
                    _CaseSensitive = value;
                    foreach (string section in this.Keys)
                    {
                        this[section].CaseSensitive = value;
                    }
                }
            }

            public bool AllowEmptySection = true;

            public bool IgnoreWhiteSpace = true;

            public string[] CommentCharacters = new string[] { ";" };

            public string[] KeyValueSeparators = new string[] { "=" };

            public string[] SectionBrackets = new string[] { "[]" };

            public bool Load(string fileName)
            {
                string content = System.IO.File.ReadAllText(fileName);
                return Parse(content);
            }

            public bool Load(string fileName, Encoding encoding)
            {
                string content = System.IO.File.ReadAllText(fileName, encoding);
                Clear();
                return Parse(content);
            }

            public bool Parse(string content)
            {
                if (null == content)
                {
                    return false;
                }
                //string[] lines = content.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
                string[] lines = Regex.Split(content, @"\r\n|\n|\r");
                string section = "";
                List<string> left = new List<string>();
                List<string> right = new List<string>();
                List<string> escape = new List<string>();
                for (int i = 0; i < SectionBrackets.Length; i++)
                {
                    left.Add(GetLeftBracket(SectionBrackets[i]));
                    right.Add(GetRightBracket(SectionBrackets[i]));
                    escape.Add(GetEscapeBracket(SectionBrackets[i]));
                }
                int each = -1;
                while (true)
                {
                repeat:
                    if (++each >= lines.Length)
                        break;

                    string line = lines[each];

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (IgnoreWhiteSpace)
                    {
                        line = line.Trim();
                        if (line.Length == 0)
                            continue;
                    }

                    for (int i = 0; i < CommentCharacters.Length; i++)
                    {
                        if (line.StartsWith(CommentCharacters[i]))
                            goto repeat;
                    }

                    for (int i = 0; i < SectionBrackets.Length; i++)
                    {
                        bool check = CheckIfInBrackets(line, left[i], right[i]);
                        if (check)
                        {
                            section = StripBrackets(line, left[i], right[i], escape[i]);
                            if (section.Length == 0 && !AllowEmptySection)
                                goto repeat;
                            Add(section);
                            goto repeat;
                        }
                    }

                    for (int i = 0; i < KeyValueSeparators.Length; i++)
                    {
                        string separator = KeyValueSeparators[i];
                        if (line.Contains(separator))
                        {
                            int p = line.IndexOf(separator);
                            string key = line.Substring(0, p);
                            string value = line.Substring(p + 1, line.Length - p - 1);
                            if (IgnoreWhiteSpace)
                            {
                                key = key.TrimEnd();
                                value = key.TrimStart();
                            }
                            Add(section, key, value);
                            goto repeat;
                        }
                    }

                    Add(section, line, "");
                }
                return true;
            }

            public Dictionary<string, string> Add(string section)
            {
                if (base.ContainsKey(section))
                    return this[section];
                Energy.Base.Collection.StringDictionary list = new Energy.Base.Collection.StringDictionary();
                this.Add(section, list);
                return list;
            }

            public Dictionary<string, string> Add(string section, string key, string value)
            {
                Dictionary<string, string> list = Add(section);
                list[key] = value;
                return list;
            }

            private string StripBrackets(string text, string left, string right, string escape)
            {
                if (string.IsNullOrEmpty(text))
                    return text;
                return text.Substring(left.Length, text.Length - left.Length - right.Length).Replace(escape, right);
            }

            private bool CheckIfInBrackets(string text, string left, string right)
            {
                if (string.IsNullOrEmpty(text))
                    return false;
                return text.StartsWith(left) && text.EndsWith(right);
            }

            private string GetLeftBracket(string bracket)
            {
                if (string.IsNullOrEmpty(bracket))
                    return bracket;
                if (bracket.Length % 2 != 0)
                    return bracket;
                return bracket.Substring(0, bracket.Length / 2);
            }

            private string GetRightBracket(string bracket)
            {
                if (string.IsNullOrEmpty(bracket))
                    return bracket;
                if (bracket.Length % 2 != 0)
                    return bracket;
                int length = bracket.Length / 2;
                return bracket.Substring(length, length);
            }

            private string GetEscapeBracket(string bracket)
            {
                if (string.IsNullOrEmpty(bracket))
                    return bracket;
                if (bracket.Length % 2 != 0)
                    return bracket + bracket;
                int length = bracket.Length / 2;
                string right = bracket.Substring(length, length);
                return right + right;
            }

            public new string ToString()
            {
                List<string> lines = new List<string>();
                string left = GetLeftBracket(SectionBrackets[0]);
                string right = GetRightBracket(SectionBrackets[0]);
                string escape = GetEscapeBracket(SectionBrackets[0]);
                string separator = KeyValueSeparators[0];
                bool empty = true;
                foreach (string section in Keys)
                {
                    if (!empty)
                        lines.Add("");
                    if (section.Length > 0)
                        lines.Add(string.Concat(left, section.Replace(right, escape), right));
                    empty = false;

                    foreach (string key in this[section].Keys)
                    {
                        string value = this[section][key];
                        if (string.IsNullOrEmpty(value))
                            lines.Add(key);
                        else
                            lines.Add(string.Concat(key, separator, value));
                    }
                }
                return string.Join(Energy.Base.Text.NL, lines.ToArray());
            }
        }
    }
}
