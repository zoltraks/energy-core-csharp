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
        #region Constant

        public const string BEGIN_ARRAY = "[";
        public const string BEGIN_OBJECT = "{";
        public const string END_ARRAY = "]";
        public const string END_OBJECT = "}";
        public const string NAME_SEPARATOR = ":";
        public const string VALUE_SEPARATOR = ",";

        #endregion

        #region Document

        public class Document: JsonValue
        {
            JsonValue _Root;

            public JsonValue Root { get { return _Root; } set { _Root = value; } }

            public override string GetText(JsonLayout layout)
            {
                return _Root.GetText(layout);
            }

            public string Serialize(JsonLayout layout)
            {
                return _Root.GetText(layout);
            }
        }

        #endregion

        #region Type

        #region JsonValue

        public abstract class JsonValue
        {
            public abstract string GetText(JsonLayout layout);

            public override string ToString()
            {
                return GetText(JsonLayout.Default);
            }

            public virtual string ToString(JsonLayout layout)
            {
                return GetText(layout);
            }
        }

        #endregion

        public class JsonString : JsonValue
        {
            private string _Text;

            private string _Value;

            public string Text
            {
                get
                {
                    return GetText(null);
                }
                set
                {
                    SetText(value);
                }
            }

            public override string GetText(JsonLayout layout)
            {
                if (_Text == null)
                {
                    _Text = Energy.Base.Json.Quote(_Value);
                }
                return _Text;
            }

            private void SetText(string value)
            {
                _Text = value;
                _Value = null;
            }

            public string Value
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

            private void SetValue(string value)
            {
                _Value = value;
                _Text = null;
            }

            private string GetValue()
            {
                return _Value;
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
                        s.Append("[");
                        if (!layout.Compact)
                            s.Append(" ");
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

        public class JsonObject : JsonValue
        {
            public Energy.Base.Collection.StringDictionary<JsonValue> Dictionary = new Energy.Base.Collection.StringDictionary<JsonValue>();

            public override string GetText(JsonLayout layout)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BEGIN_OBJECT);
                if (layout.Indent)
                {
                    layout.IndentLevel++;
                }
                else
                    sb.Append(layout.Compact ? "" : " ");
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, JsonValue> e in Dictionary)
                {
                    string pair = Energy.Base.Json.Quote(e.Key);
                    pair += NAME_SEPARATOR;
                    if (!layout.Compact)
                        pair += " ";
                    pair += e.Value.GetText(layout);
                    list.Add(pair);
                }
                string glue = VALUE_SEPARATOR;

                if (layout.Indent)
                {
                    glue += "\n";
                    glue += layout.IndentString ?? "";
                }
                else if (!layout.Compact)
                    glue += " ";

                if (list.Count > 0)
                {
                    if (layout.Indent)
                    {
                        sb.AppendLine();
                        sb.Append(layout.IndentString ?? "");
                    }
                    sb.Append(string.Join(glue, list.ToArray()));
                }
                else
                {
                    if (!layout.Compact && !layout.Indent)
                    {
                        sb.Append(" ");
                    }
                }
                sb.Append(END_OBJECT);
                if (layout.Indent)
                {
                    layout.IndentLevel--;
                }

                return sb.ToString();
            }
        }

        #endregion

        #region JsonLayout

        public class JsonLayout : Energy.Base.Pattern.DefaultProperty<JsonLayout>
        {
            /// <summary>
            /// Use indent
            /// </summary>
            public bool Indent;

            /// <summary>
            /// Be compact
            /// </summary>
            public bool Compact;

            public bool AllowCommaExtra;

            public bool AllowCommaEmpty;

            /// <summary>
            /// Write array values in single line instead of each line for each element
            /// </summary>
            public bool InlineArray;

            /// <summary>
            /// Write object values in single line instead of each line for key value pair
            /// </summary>
            public bool InlineObject;

            public int IndentLevel;

            /// <summary>
            /// Indent with
            /// </summary>
            public string IndentString;

            public JsonLayout()
            {
                Reset();
            }

            public void Reset()
            {
                this.Compact = false;
                this.IndentLevel = 0;
                this.Indent = true;
                this.IndentString = "\t";
                this.AllowCommaExtra = false;
                this.AllowCommaEmpty = false;
                this.InlineArray = false;
                this.InlineObject = false;
            }
        }

        #endregion

        #region Static

        private static readonly string[] characterReplacementArray = new string[]{
            "\b", "\\b",
            "\f", "\\f",
            "\n", "\\n",
            "\r", "\\r",
            "\t", "\\t",
            "\"", "\\\"",
            "\\", "\\\\",
        };

        #region Escape

        /// <summary>
        /// Escape special characters for JSON value.
        /// 
        /// Backspace is replaced with '\b'.
        /// Form feed is replaced with '\f'.
        /// Newline is replaced with '\n'.
        /// Carriage return is replaced with '\r'.
        /// Tab is replaced with '\t'.
        /// Double quote is replaced with '\"'.
        /// Backslash is replaced with '\\'.
        /// 
        /// Escaped text must be surrounded with double quotes.
        /// </summary>
        /// <param name="text">Text to be escaped</param>
        /// <returns>JSON string</returns>
        public static string Escape(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            for (int i = characterReplacementArray.Length - 2; i >= 0; i -= 2)
            {
                if (text.Contains(characterReplacementArray[i]))
                {
                    text = text.Replace(characterReplacementArray[i], characterReplacementArray[i + 1]);
                }
            }
            return text;
        }

        #endregion

        #region Unescape

        /// <summary>
        /// Strip backslashes from previously escaped value.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Unescape(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            for (int i = 0; i < characterReplacementArray.Length; i += 2)
            {
                if (text.Contains(characterReplacementArray[i + 1]))
                {
                    text = text.Replace(characterReplacementArray[i + 1], characterReplacementArray[i]);
                }
            }
            return text;
        }

        #endregion

        #region Strip

        /// <summary>
        /// Strip double quotes and unescapes string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strip(string text)
        {
            if (text == null)
            {
                return null;
            }
            if (text == "null")
            {
                return null;
            }
            if (text == "")
            {
                return "";
            }
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2);
                text = Energy.Base.Json.Unescape(text);
            }
            return text;
        }

        #endregion

        #region Quote

        /// <summary>
        /// Quote text. 
        /// Represents text in double quotes and escapes special characters.
        /// Null strings are represented as single word "null".
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Quote(string text)
        {
            return text == null ? "null" : "\"" + Escape(text) + "\"";
        }

        #endregion

        #endregion
    }
}
