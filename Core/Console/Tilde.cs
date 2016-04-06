using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Energy.Enumeration;

namespace Energy.Console
{
    /// <summary>
    /// Simple elegant text coloring engine for console programs. Based on Empty Page #0 color engine.
    /// </summary>
    public class Tilde
    {
        /// <summary>
        /// List of texts with different coloring
        /// </summary>
        public class TextList : List<TextList.Item>
        {
            /// <summary>
            /// Item
            /// </summary>
            public class Item
            {
                /// <summary>
                /// Color
                /// </summary>
                public ConsoleColor Color;

                /// <summary>
                /// Text
                /// </summary>
                public string Text;
            }

            /// <summary>
            /// Explode text to a list separated by color change
            /// </summary>
            /// <param name="message">Text</param>
            /// <returns>List of texts</returns>
            public static TextList Explode(string message)
            {
                if (message == null) return null;
                TextList list = new TextList();
                Regex r = new Regex(@"~\d+~|~[\w\d]+~|~+|[^~]+");
                Match m = r.Match(message);
                System.ConsoleColor current = System.ConsoleColor.Black;
                while (m.Success)
                {
                    if (m.Value.StartsWith("~") && m.Value.EndsWith("~") && m.Value.Length > 2 && m.Value[1] != '~')
                    {
                        switch (m.Value.Substring(1, m.Value.Length - 2).ToLower())
                        {
                            default:
                            case "0":
                            case "default":
                                current = System.ConsoleColor.Black;
                                break;
                            case "1":
                            case "darkblue":
                            case "db":
                                current = System.ConsoleColor.DarkBlue;
                                break;
                            case "2":
                            case "darkgreen":
                            case "dg":
                                current = System.ConsoleColor.DarkGreen;
                                break;
                            case "3":
                            case "darkcyan":
                            case "dc":
                                current = System.ConsoleColor.DarkCyan;
                                break;
                            case "4":
                            case "darkred":
                            case "dr":
                                current = System.ConsoleColor.DarkRed;
                                break;
                            case "5":
                            case "darkmagenta":
                            case "dm":
                                current = System.ConsoleColor.DarkMagenta;
                                break;
                            case "6":
                            case "darkyellow":
                            case "dy":
                                current = System.ConsoleColor.DarkYellow;
                                break;
                            case "7":
                            case "gray":
                                current = System.ConsoleColor.Gray;
                                break;
                            case "8":
                            case "darkgray":
                                current = System.ConsoleColor.DarkGray;
                                break;
                            case "9":
                            case "blue":
                            case "b":
                                current = System.ConsoleColor.Blue;
                                break;
                            case "10":
                            case "green":
                            case "g":
                                current = System.ConsoleColor.Green;
                                break;
                            case "11":
                            case "cyan":
                            case "c":
                                current = System.ConsoleColor.Cyan;
                                break;
                            case "12":                            
                            case "red":
                            case "r":
                                current = System.ConsoleColor.Red;
                                break;
                            case "13":
                            case "magenta":
                            case "m":
                                current = System.ConsoleColor.Magenta;
                                break;
                            case "14":
                            case "yellow":
                            case "y":
                                current = System.ConsoleColor.Yellow;
                                break;
                            case "15":
                            case "white":
                            case "w":
                                current = System.ConsoleColor.White;
                                break;
                        }
                    }
                    else
                    {
                        list.Add(new Item() { Color = current, Text = m.Value });
                    }
                    m = m.NextMatch();
                }
                return list;
            }
        }

        public static string Ask(string question, CharacterCasing casing, string value = "")
        {
            string answer = Ask(question);
            if (String.IsNullOrEmpty(answer))
                return value;
            switch (casing)
            {
                default:
                    return answer;
                case CharacterCasing.Lower:
                    return answer.ToLower();
                case CharacterCasing.Upper:
                    return answer.ToUpper();
            }
        }

        /// <summary>
        /// Break new line and set default text color
        /// </summary>
        /// <param name="count"></param>
        public static void Break(int count = 1)
        {
            if (defaultForeground != null) System.Console.ForegroundColor = (ConsoleColor)defaultForeground;
            for (int i = 0; i < count; i++)
            {
                System.Console.WriteLine();
            }
        }

        private static ConsoleColor? defaultForeground;

        /// <summary>
        /// Write text using color settings ~white~, ~15~, ~14~, ~13~, ..., ~0~
        /// </summary>
        /// <example>
        /// Tilde.Write(Tilde.ExampleColorPalleteTildeString);
        /// </example>
        /// <param name="value"></param>
        public static void Write(string value)
        {
            if (value == null) return;
            // fix new lines :)
            //value = value.Replace("\r\n", "\n").Replace("\n", "\r\n");
            if (defaultForeground == null)
            {
                defaultForeground = System.Console.ForegroundColor;
            }
            TextList list = TextList.Explode(value);
            for (int i = 0; i < list.Count; i++)
            {
                System.Console.ForegroundColor = list[i].Color != ConsoleColor.Black ? list[i].Color : (ConsoleColor)defaultForeground;
                System.Console.Write(list[i].Text);
            }
            //System.Console.ForegroundColor = foreground;
        }

        /// <summary>
        /// Write text line using color settings ~white~, ~15~, ~14~, ~13~, ..., ~0~
        /// <br />
        /// After print console color settings will remain the same.
        /// <code>
        ///     Energy.Console.Tilde.WriteLine("~lightgreen~Hello ~white~World~lightred~!");
        /// </code>
        /// </summary>
        /// <example>
        /// <code>
        ///     Energy.Console.Tilde.WriteLine("~lightgreen~Hello ~white~World~lightred~!");
        /// </code>
        /// </example>
        /// <param name="value"></param>
        public static void WriteLine(string value)
        {
            Write(value);
            Break();
        }

        /// <summary>
        /// Cheat sheet for all colors defined by default
        /// </summary>
        public readonly static string ExampleColorPalleteTildeString = ""
            + "~darkblue~ ~ darkblue ~   ~ 1 ~   "
            + "~darkgreen~ ~ darkgreen ~  ~ 2 ~   "
            + "~darkcyan~ ~ darkcyan ~  ~ 3 ~    "
            + "~darkred~ ~ darkred ~  ~ 4 ~    "
            + "~darkmagenta~ ~ darkmagenta ~  ~ 5 ~    "
            + "~darkyellow~ ~ darkyellow ~  ~ 6 ~    "
            + "~gray~ ~ gray ~  ~ 7 ~    "
            + "~darkgray~ ~ darkgray ~  ~ 8 ~    "
            + "~blue~ ~ blue ~  ~ 9 ~    "
            + "~green~ ~ green ~  ~ 10 ~    "
            + "~cyan~ ~ cyan ~  ~ 11 ~    "
            + "~red~ ~ red ~  ~ 12 ~    "
            + "~magenta~ ~ magenta ~  ~ 13 ~    "
            + "~yellow~ ~ yellow ~  ~ 14 ~    "
            + "~white~ ~ white ~  ~ 15 ~    "
            ;

        /// <summary>
        /// Ask question with optional default value. Default will be returned if skipped by entering empty value.
        /// </summary>
        /// <param name="question">Question string</param>
        /// <param name="value">Default value</param>
        /// <returns>Value entered or default if skipped. Default is an empty string.</returns>
        public static string Ask(string question, string value = "")
        {
            Tilde.Write("~15~" + question + (String.IsNullOrEmpty(value) ? "" : "~13~" + " [ " + "~9~" + value + "~13~" + " ]") + "~0~" + " : ");
            int left = System.Console.CursorLeft;
            System.ConsoleColor foreground = System.Console.ForegroundColor;
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            string answer = System.Console.ReadLine();
            int top = System.Console.CursorTop;
            System.Console.ForegroundColor = foreground;
            if (answer.Length == 0)
            {
                answer = value;
                try
                {
                    System.Console.SetCursorPosition(left, top - 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                System.Console.WriteLine(answer);
            }
            return answer;
        }
    }
}
