using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// JSON support
    /// </summary>
    public class Json
    {
        public abstract class JsonValue
        {
            public abstract string GetText(JsonLayout layout);

            public override string ToString()
            {
                return GetText(JsonLayout.Default);
            }
        }

        public class JsonLayout : Energy.Base.Pattern.DefaultProperty<JsonLayout>
        {
            public bool Indent;

            public bool AllowCommaExtra;

            public bool AllowCommaEmpty;

            public bool InlineArray;

            public bool InlineObject;

            public int IndentLevel;

            public string IndentString;

            public JsonLayout()
            {
                Reset();
            }

            public void Reset()
            {
                this.IndentLevel = 0;
                this.Indent = true;
                this.IndentString = "\t";
                this.AllowCommaExtra = false;
                this.AllowCommaEmpty = false;
                this.InlineArray = false;
                this.InlineObject = false;
            }
        }

        public class JsonString : JsonValue
        {
            private string _Text;

            public string Text
            {
                get
                {
                    return GetText();
                }
                set
                {
                    SetText(value);
                }
            }

            private string GetText()
            {
                return _Text;
            }

            private void SetText(string value)
            {
                _Text = value;
                _Value = null;
            }

            private object _Value;

            public object Value
            {
                get
                {
                    return GetValue();
                }
                set
                {
                    SetValue(value);
                }
            }

            private void SetValue(object value)
            {
                _Value = value;
                _Text = null;
            }

            private object GetValue()
            {
                if (!string.IsNullOrEmpty(_Text))
                {
                    return Energy.Base.Cast.JsonValueToObject(_Text);
                }
                return _Value;
            }

            public override string GetText(JsonLayout layout)
            {
                if (!string.IsNullOrEmpty(_Text))
                    return _Text;
                return Energy.Base.Cast.ObjectToJsonValue(_Value);
            }
        }

        public class JsonNumber : JsonValue
        {
            private string _Text;

            public string Text
            {
                get
                {
                    return GetText();
                }
                set
                {
                    SetText(value);
                }
            }

            private void SetText(string value)
            {
                _Text = value;
                _Value = 0;
            }

            private string GetText()
            {
                if (!string.IsNullOrEmpty(_Text))
                    return _Text;
                return _Value.ToString(CultureInfo.InvariantCulture);
            }

            private double _Value;

            public double Value
            {
                get
                {
                    return GetValue();
                }
                set
                {
                    SetValue(value);
                }
            }

            private void SetValue(double value)
            {
                _Value = value;
                _Text = null;
            }

            private double GetValue()
            {
                if (!string.IsNullOrEmpty(_Text))
                    return Energy.Base.Cast.StringToDouble(_Text);
                return _Value;
            }

            public override string GetText(JsonLayout layout)
            {
                return Text;
            }
        }

        public class JsonBoolean : JsonValue
        {
            private bool _Value;

            public bool Value
            {
                get
                {
                    return GetValue();
                }
                set
                {
                    SetValue(value);
                }
            }

            private bool GetValue()
            {
                return _Value;
            }

            private void SetValue(bool value)
            {
                _Value = value;
            }

            public override string GetText(JsonLayout layout)
            {
                return _Value ? "true" : "false";
            }
        }

        public class JsonArray : JsonValue
        {
            public List<JsonValue> Array = new List<JsonValue>();

            public override string GetText(JsonLayout layout)
            {
                if (Array.Count == 0)
                    return "[]";
                StringBuilder s = new StringBuilder();
                if (!layout.Indent)
                {
                    s.Append("[");
                    List<string> list = new List<string>();
                    for (int i = 0; i < Array.Count; i++)
                    {
                        if (Array[i] == null)
                            list.Add("null");
                        else
                            list.Add(Array[i].GetText(layout));
                    }
                    s.Append(string.Join(",", list.ToArray()));
                    s.Append("]");
                }
                else
                {
                    if (layout.InlineArray)
                    {
                        s.Append("[ ");
                        List<string> list = new List<string>();
                        for (int i = 0; i < Array.Count; i++)
                        {
                            if (Array[i] == null)
                                list.Add("null");
                            else
                                list.Add(Array[i].GetText(layout));
                        }
                        s.Append(string.Join(", ", list.ToArray()));
                        s.Append(" ]");
                    }
                    else
                    {
                        s.Append("[");
                        s.AppendLine();
                        layout.IndentLevel++;
                        for (int i = 0; i < Array.Count; i++)
                        {
                            for (int n = 0; n < layout.IndentLevel; n++)
                            {
                                s.Append(layout.IndentString);
                            }
                            if (Array[i] == null)
                                s.Append("null");
                            else
                                s.Append(Array[i].GetText(layout));
                            if (i < Array.Count - 1)
                                s.Append(",");
                            s.AppendLine();
                        }
                        layout.IndentLevel--;
                        for (int n = 0; n < layout.IndentLevel; n++)
                        {
                            s.Append(layout.IndentString);
                        }
                        s.Append("]");
                    }
                }
                return s.ToString();
            }
        }

        public class JsonObject: JsonValue
        {
            public Energy.Base.Collection.StringDictionary<JsonValue> Dictionary = new Energy.Base.Collection.StringDictionary<JsonValue>();

            public override string GetText(JsonLayout layout)
            {
                throw new NotImplementedException();
            }
        }

        private static readonly string[] characterReplacementArray = new string[]{
            "\b", "\\b",
            "\f", "\\f",
            "\n", "\\n",
            "\r", "\\r",
            "\"", "\\\"",
            "\\", "\\\\",
        };

        /// <summary>
        /// Escape JSON characters and include string in double quotes.
        /// Null strings will be represented as "null".
        /// 
        /// Backspace is replaced with \b
        /// Form feed is replaced with \f
        /// Newline is replaced with \n
        /// Carriage return is replaced with \r
        /// Tab is replaced with \t
        /// Double quote is replaced with \"
        /// Backslash is replaced with \\
        /// 
        /// </summary>
        /// <param name="text">Text to be escaped for JSON string</param>
        /// <returns>JSON string</returns>
        public static string Escape(string text)
        {
            if (text == null)
                return "null";
            if (text == "")
                return "\"\"";
            for (int i = characterReplacementArray.Length - 2; i >= 0; i -= 2)
            {
                if (text.Contains(characterReplacementArray[i]))
                    text = text.Replace(characterReplacementArray[i], characterReplacementArray[i + 1]);
            }
            return string.Concat("\"", text, "\"");
        }

        /// <summary>
        /// Strip quotes and unescapes JSON string to text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strip(string text)
        {
            if (text == null)
                return null;
            if (text == "null")
                return null;
            if (text == "")
                return "";
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2);
                for (int i = 0; i < characterReplacementArray.Length; i += 2)
                {
                    if (text.Contains(characterReplacementArray[i + 1]))
                        text = text.Replace(characterReplacementArray[i + 1], characterReplacementArray[i]);
                }
                return text;
            }
            else
            {
                return text;
            }
        }
    }
}
