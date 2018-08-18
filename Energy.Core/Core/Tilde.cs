﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Energy.Enumeration;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Simple elegant text coloring engine for console programs.
    /// Based on Empty Page #0 color engine.
    /// 
    /// Color may be specified by its number or name surrounded by tilde (~) character.
    /// One tilde followed by a character other than a letter or number or a fragment consisting of two or more tilde characters will not be formatted.
    /// 
    /// Colors that can be used are the same as on a standard command console.
    /// There are 15 different colours plus black.
    /// 
    /// Dark colour identifiers are preceded by the letter 'd' (dark).
    /// In a similar way, bright-colour identifiers can be preceded by the letter 'l' (light).
    /// 
    /// Color changes one by one are ignored.Only the last defined color will be used.
    /// </summary>
    /// <remarks>
    /// List of available colors:
    /// <br/>
    /// <table>
    /// <thead>
    ///     <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
    /// </thead>
    /// <tbody>
    /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
    /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
    /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
    /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
    /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
    /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
    /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
    /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
    /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
    /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
    /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
    /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
    /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
    /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
    /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
    /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
    /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
    /// </tbody>
    /// </table>
    /// </remarks>
    public static class Tilde
    {
        #region Class

        /// <summary>
        /// List of texts with different coloring
        /// </summary>
        public class ColorTextList : List<ColorTextList.Item>
        {
            /// <summary>
            /// Item
            /// </summary>
            public class Item
            {
                /// <summary>
                /// Color
                /// </summary>
                public ConsoleColor? Color;

                /// <summary>
                /// Text
                /// </summary>
                public string Text;
            }

            /// <summary>
            /// Total count of characters
            /// </summary>
            public int TotalLength
            {
                get
                {
                    return GetTotalLength();
                }
            }

            /// <summary>
            /// Join list of string with glue
            /// </summary>
            /// <param name="glue"></param>
            /// <returns></returns>
            public string Join(string glue)
            {
                List<string> list = new List<string>();
                for (int i = 0; i < this.Count; i++)
                {
                    if (string.IsNullOrEmpty(this[i].Text))
                        continue;
                    list.Add(this[i].Text);
                }
                return string.Join(glue, list.ToArray());
            }

            /// <summary>
            /// Get total length
            /// </summary>
            /// <returns></returns>
            public int GetTotalLength()
            {
                int count = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Text == null)
                        continue;
                    count += this[i].Text.Length;
                }
                return count;
            }

            /// <summary>
            /// Explode text to a list separated by color change
            /// </summary>
            /// <param name="message">Text</param>
            /// <returns>List of texts</returns>
            public static ColorTextList Explode(string message)
            {
                if (message == null)
                    return null;
                ColorTextList list = new ColorTextList();
                Match m = Regex.Match(message, Energy.Base.Expression.TildeText);
                System.ConsoleColor? current = null;
                while (m.Success)
                {
                    string value = m.Value;
                    if (value.Length >= 4 && value.StartsWith("~`") && value.EndsWith("`~"))
                    {
                        list.Add(new ColorTextList.Item()
                        {
                            Color = current
                            ,
                            Text = value.Substring(2, value.Length - 4).Replace("``", "`")
                        });
                    }
                    else if (value.Length > 2 && value.StartsWith("~") && value.EndsWith("~") && value[1] != '~')
                    {
                        current = TildeColorToConsoleColor(value);
                    }
                    else
                    {
                        list.Add(new ColorTextList.Item()
                        {
                            Color = current
                            ,
                            Text = value
                        });
                    }
                    m = m.NextMatch();
                }
                return list;
            }
        }

        #endregion

        #region Global

        private static readonly object _ConsoleLock = new object();

        private static ConsoleColor? _Foreground;
        /// <summary>
        /// Foreground color
        /// </summary>
        public static ConsoleColor? Foreground { get { return _Foreground; } set { _Foreground = value; } }

        private static bool _Colorless;
        /// <summary>Disable console colors</summary>
        public static bool Colorless
        {
            get
            {
                return _Colorless;
            }
            set
            {
                if (value == _Colorless)
                    return;
                SetColorless(value);
            }
        }

        /// <summary>
        /// Set Colorless property value.
        /// </summary>
        /// <param name="value"></param>
        public static void SetColorless(bool value)
        {
            _Colorless = value;
        }

        public const string DefaultPauseText = "Enter ~w~anything~0~ to ~y~continue~0~...";
        private static string _PauseText = DefaultPauseText;
        public static string PauseText { get { return _PauseText; } set { _PauseText = value; } }

        public const string DefaultAskSimpleText = "~15~{0}~0~ : ";
        private static string _AskSimpleText = DefaultAskSimpleText;
        public static string AskSimpleText { get { return _AskSimpleText; } set { _AskSimpleText = value; } }

        public const string DefaultAskChangeText = "~15~{0}~0~ ~13~[~0~ ~11~{1}~0~ ~13~]~0~ : ";
        private static string _AskChangeText = DefaultAskChangeText;
        public static string AskChangeText { get { return _AskChangeText; } set { _AskChangeText = value; } }

        private static string _ExampleColorPalleteTildeString = ""
            + "~darkblue~~#~1~#~\t~#~darkblue~#~"
            + Environment.NewLine
            + "~darkgreen~~#~2~#~\t~#~darkgreen~#~"
            + Environment.NewLine
            + "~darkcyan~~#~3~#~\t~#~darkcyan~#~"
            + Environment.NewLine
            + "~darkred~~#~4~#~\t~#~darkred~#~"
            + Environment.NewLine
            + "~darkmagenta~~#~5~#~\t~#~darkmagenta~#~"
            + Environment.NewLine
            + "~darkyellow~~#~6~#~\t~#~darkyellow~#~"
            + Environment.NewLine
            + "~gray~~#~7~#~\t~#~gray~#~"
            + Environment.NewLine
            + "~darkgray~~#~8~#~\t~#~darkgray~#~"
            + Environment.NewLine
            + "~blue~~#~9~#~\t~#~blue~#~"
            + Environment.NewLine
            + "~green~~#~10~#~\t~#~green~#~"
            + Environment.NewLine
            + "~cyan~~#~11~#~\t~#~cyan~#~"
            + Environment.NewLine
            + "~red~~#~12~#~\t~#~red~#~"
            + Environment.NewLine
            + "~magenta~~#~13~#~\t~#~magenta~#~"
            + Environment.NewLine
            + "~yellow~~#~14~#~\t~#~yellow~#~"
            + Environment.NewLine
            + "~white~~#~15~#~\t~#~white~#~"
            + Environment.NewLine
            + "~black~~#~16~#~\t~#~black~#~"
            ;

        /// <summary>
        /// Cheat sheet for all colors defined by default
        /// </summary>
        public static string ExampleColorPalleteTildeString { get { return _ExampleColorPalleteTildeString; } set { _ExampleColorPalleteTildeString = value; } }

        #endregion

        #region Ask

        /// <summary>
        /// Ask question with optional default value. Default will be returned if skipped by entering empty value.
        /// </summary>
        /// <param name="question">Question string</param>
        /// <param name="value">Default value</param>
        /// <returns>Value entered or default if skipped</returns>
        public static string Ask(string question, string value)
        {
            if (!string.IsNullOrEmpty(question))
            {
                if (value == null)
                    value = "";
                string message = value == "" ? AskSimpleText : AskChangeText;
                message = string.Format(message, question, value);
                Tilde.Write(message);
            }
            int left = System.Console.CursorLeft;
            System.ConsoleColor foreground = System.Console.ForegroundColor;
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            string answer = System.Console.ReadLine().Trim();
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

        /// <summary>
        /// Ask question with optional default value. Default will be returned if skipped by entering empty value.
        /// </summary>
        /// <param name="question">Question string</param>
        /// <returns>Value entered or default if skipped, empty string by default</returns>
        public static string Ask(string question)
        {
            return Ask(question, "");
        }

        /// <summary>
        /// Ask question with optional default value. Default will be returned if skipped by entering empty value.
        /// Return input value lower or upper case.
        /// </summary>
        /// <param name="question">Question string</param>
        /// <param name="casing">Character casing</param>
        /// <param name="value">Default value</param>
        /// <returns>Value entered or default if skipped</returns>
        public static string Ask(string question, CharacterCasing casing, string value)
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
        /// Ask question with optional default value. Default will be returned if skipped by entering empty value.
        /// Return input value lower or upper case.
        /// </summary>
        /// <param name="question">Question string</param>
        /// <param name="casing">Character casing</param>
        /// <returns>Value entered or default if skipped, empty string by default</returns>
        public static string Ask(string question, CharacterCasing casing)
        {
            return Ask(question, casing, "");
        }

        public static object Ask(string question, object value)
        {
            string text = Energy.Base.Cast.As<string>(value);
            string answer = Ask(question, text);
            if (String.IsNullOrEmpty(answer))
                return value;
            return answer;
        }

        #endregion

        #region Break

        /// <summary>
        /// Break with new lines and set default text color.
        /// </summary>
        /// <param name="count"></param>
        public static void Break(int count)
        {
            lock (_ConsoleLock)
            {
                if (_Foreground != null)
                    System.Console.ForegroundColor = (ConsoleColor)_Foreground;
                for (int i = 0; i < count; i++)
                {
                    System.Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Break one line and set default text color.
        /// </summary>
        public static void Break()
        {
            Break(1);
        }

        /// <summary>
        /// Break with one or more padding lines and ruler line.
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="line"></param>
        public static void Break(int padding, string line)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < padding; i++)
                s.AppendLine();
            string x = s.ToString();
            string t = string.Concat(x, line, x, Environment.NewLine);
            Write(t);
        }

        #endregion

        #region Write

        /// <summary>
        /// <para>
        /// Write color text containing modifiers like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
        /// </para>
        /// <para>
        /// <code>
        ///     Energy.Core.Tilde.Write("~red~Breaking: ~white~Hell, ~yellow~yea.");
        /// </code>
        /// </para>
        /// <para>List of available console colors:</para>
        /// <para>~0~, ~default~ = Default color</para>
        /// <para>~1~, ~darkblue~, ~db~ = Dark blue</para>
        /// <para>~2~, ~darkgreen~, ~dg~, Dark green</para>
        /// <para>~3~, ~darkcyan~, ~dc~, Dark cyan</para>
        /// <para>~4~, ~darkred~, ~dr~, Dark red</para>
        /// <para>~5~, ~darkmagenta~, ~dm~, Dark magenta</para>
        /// <para>~6~, ~darkyellow~, ~dy~, Dark yellow</para>
        /// <para>~7~, ~gray~, ~s~<br/>~ls~, Gray / Silver</para>
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray / Dark silver</para>
        /// <para>~9~, ~blue~, ~b~, Blue</para>
        /// <para>~10~, ~green~, ~g~, Green</para>
        /// <para>~11~, ~cyan~, ~c~, Cyan</para>
        /// <para>~12~, ~red~, ~r~, Red</para>
        /// <para>~13~, ~magenta~, ~m~, Magenta</para>
        /// <para>~14~, ~yellow~, ~y~, Yellow</para>
        /// <para>~15~, ~white~, ~w~, White</para>
        /// <para>~16~, ~black~, ~k~, Black / Carbon</para>
        /// </summary>
        /// <remarks>
        /// List of available colors:
        /// <br/>
        /// <table>
        /// <thead>
        ///     <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
        /// </thead>
        /// <tbody>
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
        /// </tbody>
        /// </table>
        /// </remarks>
        /// <example>
        /// <code>
        ///     Energy.Core.Tilde.Write("~red~Breaking: ~white~Hell, ~yellow~yea.");
        /// </code>
        /// </example>
        /// <param name="value"></param>
        public static void Write(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            ColorTextList list = ColorTextList.Explode(value);
            if (list.Count == 0)
                return;
            lock (_ConsoleLock)
            {
                // TODO Fix new lines :)
                // TODO Optionally wrap words?
                // TODO Check security
                System.ConsoleColor previousForegroundColor = System.Console.ForegroundColor;
                System.ConsoleColor defaultForegroundColor = _Foreground != null
                    ? (ConsoleColor)_Foreground
                    : previousForegroundColor;
                for (int i = 0; i < list.Count; i++)
                {
                    if (!_Colorless)
                    {
                        System.ConsoleColor foregroundColor = list[i].Color != null
                            ? (ConsoleColor)list[i].Color
                            : defaultForegroundColor;
                        try
                        {
                            System.Console.ForegroundColor = foregroundColor;
                        }
                        catch (System.Security.SecurityException)
                        {
                            _Colorless = true;
                        }
                    }
                    System.Console.Write(list[i].Text);
                }
                if (!_Colorless)
                {
                    System.Console.ForegroundColor = previousForegroundColor;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Write color text with line ending containing modifiers
        /// like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
        /// </para>
        /// <para>
        /// <code>
        ///     Energy.Core.Tilde.WriteLine("~red~Breaking: ~white~Hell, ~yellow~yea.");
        /// </code>
        /// </para>
        /// <para>List of available console colors:</para>
        /// <para>~0~, ~default~ = Default color</para>
        /// <para>~1~, ~darkblue~, ~db~ = Dark blue</para>
        /// <para>~2~, ~darkgreen~, ~dg~, Dark green</para>
        /// <para>~3~, ~darkcyan~, ~dc~, Dark cyan</para>
        /// <para>~4~, ~darkred~, ~dr~, Dark red</para>
        /// <para>~5~, ~darkmagenta~, ~dm~, Dark magenta</para>
        /// <para>~6~, ~darkyellow~, ~dy~, Dark yellow</para>
        /// <para>~7~, ~gray~, ~s~<br/>~ls~, Gray / Silver</para>
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray / Dark silver</para>
        /// <para>~9~, ~blue~, ~b~, Blue</para>
        /// <para>~10~, ~green~, ~g~, Green</para>
        /// <para>~11~, ~cyan~, ~c~, Cyan</para>
        /// <para>~12~, ~red~, ~r~, Red</para>
        /// <para>~13~, ~magenta~, ~m~, Magenta</para>
        /// <para>~14~, ~yellow~, ~y~, Yellow</para>
        /// <para>~15~, ~white~, ~w~, White</para>
        /// <para>~16~, ~black~, ~k~, Black / Carbon</para>
        /// </summary>
        /// <remarks>
        /// List of available colors:
        /// <br/>
        /// <table>
        /// <thead>
        ///     <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
        /// </thead>
        /// <tbody>
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
        /// </tbody>
        /// </table>
        /// </remarks>
        /// <example>
        /// <code>
        ///     Energy.Console.Tilde.WriteLine("~lightgreen~Hello ~white~World~lightred~!");
        /// </code>
        /// </example>
        /// <param name="format"></param>
        /// <param name="value"></param>
        public static void Write(string format, params object[] value)
        {
            Write(string.Format(format, value));
        }

        /// <summary>
        /// <para>
        /// Write color text with line ending containing modifiers using specified format information
        /// like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
        /// </para>
        /// <para>
        /// <code>
        ///     Energy.Core.Tilde.WriteLine("~red~Breaking: ~white~Hell, ~yellow~yea from {0}.", "Me");
        /// </code>
        /// </para>
        /// <para>List of available console colors:</para>
        /// <para>~0~, ~default~ = Default color</para>
        /// <para>~1~, ~darkblue~, ~db~ = Dark blue</para>
        /// <para>~2~, ~darkgreen~, ~dg~, Dark green</para>
        /// <para>~3~, ~darkcyan~, ~dc~, Dark cyan</para>
        /// <para>~4~, ~darkred~, ~dr~, Dark red</para>
        /// <para>~5~, ~darkmagenta~, ~dm~, Dark magenta</para>
        /// <para>~6~, ~darkyellow~, ~dy~, Dark yellow</para>
        /// <para>~7~, ~gray~, ~s~<br/>~ls~, Gray / Silver</para>
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray / Dark silver</para>
        /// <para>~9~, ~blue~, ~b~, Blue</para>
        /// <para>~10~, ~green~, ~g~, Green</para>
        /// <para>~11~, ~cyan~, ~c~, Cyan</para>
        /// <para>~12~, ~red~, ~r~, Red</para>
        /// <para>~13~, ~magenta~, ~m~, Magenta</para>
        /// <para>~14~, ~yellow~, ~y~, Yellow</para>
        /// <para>~15~, ~white~, ~w~, White</para>
        /// <para>~16~, ~black~, ~k~, Black / Carbon</para>
        /// </summary>
        /// <remarks>
        /// List of available colors:
        /// <br/>
        /// <table>
        /// <thead>
        ///     <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
        /// </thead>
        /// <tbody>
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
        /// </tbody>
        /// </table>
        /// </remarks>
        /// <example>
        /// <code>
        ///     Energy.Console.Tilde.WriteLine("~lightgreen~Hello ~white~World~lightred~!");
        /// </code>
        /// </example>
        /// <param name="format"></param>
        /// <param name="value"></param>
        public static void WriteLine(string format, params object[] value)
        {
            Write(string.Concat(string.Format(format, value), Environment.NewLine));
        }

        /// <summary>
        /// <para>
        /// Write color text with line ending containing modifiers
        /// like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
        /// </para>
        /// <para>
        /// <code>
        ///     Energy.Core.Tilde.WriteLine("~red~Breaking: ~white~Hell, ~yellow~yea.");
        /// </code>
        /// </para>
        /// <para>List of available console colors:</para>
        /// <para>~0~, ~default~ = Default color</para>
        /// <para>~1~, ~darkblue~, ~db~ = Dark blue</para>
        /// <para>~2~, ~darkgreen~, ~dg~, Dark green</para>
        /// <para>~3~, ~darkcyan~, ~dc~, Dark cyan</para>
        /// <para>~4~, ~darkred~, ~dr~, Dark red</para>
        /// <para>~5~, ~darkmagenta~, ~dm~, Dark magenta</para>
        /// <para>~6~, ~darkyellow~, ~dy~, Dark yellow</para>
        /// <para>~7~, ~gray~, ~s~<br/>~ls~, Gray / Silver</para>
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray / Dark silver</para>
        /// <para>~9~, ~blue~, ~b~, Blue</para>
        /// <para>~10~, ~green~, ~g~, Black</para>
        /// <para>~11~, ~cyan~, ~c~, Cyan</para>
        /// <para>~12~, ~red~, ~r~, Red</para>
        /// <para>~13~, ~magenta~, ~m~, Magenta</para>
        /// <para>~14~, ~yellow~, ~y~, Yellow</para>
        /// <para>~15~, ~white~, ~w~, White</para>
        /// <para>~16~, ~black~, ~k~, Black / Carbon</para>
        /// </summary>
        /// <remarks>
        /// List of available colors:
        /// <br/>
        /// <table>
        /// <thead>
        ///     <th>Numeric</th><th>Long</th>           <th>Short</th>   <th></th>
        /// </thead>
        /// <tbody>
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>        <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>    <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>    <td>Dark green</td></tr>
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>    <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>    <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>    <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>    <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~</td>     <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>    <td>Dark gray / Dark silver</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>     <td>Blue</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>     <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>     <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>     <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>     <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>     <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>     <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>     <td>Black / Carbon</td></tr>
        /// </tbody>
        /// </table>
        /// </remarks>
        /// <example>
        /// <code>
        ///     Energy.Console.Tilde.WriteLine("~lightgreen~Hello ~white~World~lightred~!");
        /// </code>
        /// </example>
        /// <param name="value"></param>
        public static void WriteLine(string value)
        {
            Write(string.Concat(value, Environment.NewLine));
        }

        /// <summary>
        /// Write text using specifed color and escaping any tilde characters in text
        /// </summary>
        /// <param name="color"></param>
        /// <param name="text"></param>
        public static void WriteLine(ConsoleColor color, string text)
        {
            Write(string.Concat(ConsoleColorToTildeColor(color), Escape(text)
                , Environment.NewLine));
        }

        #endregion

        #region Color

        public class Color
        {
            public static string DarkBlue = "~1~";
            public static string DarkGreen = "~2~";
            public static string DarkCyan = "~3~";
            public static string DarkRed = "~4~";
            public static string DarkMagenta = "~5~";
            public static string DarkYellow = "~6~";
            public static string Gray = "~7~";
            public static string DarkGray = "~8~";
            public static string Blue = "~9~";
            public static string Green = "~10~";
            public static string Cyan = "~11~";
            public static string Red = "~12~";
            public static string Magenta = "~13~";
            public static string Yellow = "~14~";
            public static string White = "~15~";
            public static string Black = "~16~";
        }

        public static string ConsoleColorToTildeColor(ConsoleColor color)
        {
            if (color == ConsoleColor.Black)
                return "~16~";

            return string.Concat("~", (int)color, "~");
        }

        public static ConsoleColor? TildeColorToConsoleColor(string tilde)
        {
            if (string.IsNullOrEmpty(tilde))
                return null;

            switch (tilde.ToLower())
            {
                default:
                case "~0~":
                case "~default~":
                    return null;

                case "~1~":
                case "~darkblue~":
                case "~db~":
                    return System.ConsoleColor.DarkBlue;

                case "~2~":
                case "~darkgreen~":
                case "~dg~":
                    return System.ConsoleColor.DarkGreen;

                case "~3~":
                case "~darkcyan~":
                case "~dc~":
                    return System.ConsoleColor.DarkCyan;

                case "~4~":
                case "~darkred~":
                case "~dr~":
                    return System.ConsoleColor.DarkRed;

                case "~5~":
                case "~darkmagenta~":
                case "~dm~":
                    return System.ConsoleColor.DarkMagenta;

                case "~6~":
                case "~darkyellow~":
                case "~dy~":
                    return System.ConsoleColor.DarkYellow;

                case "~7~":
                case "~gray~":
                case "~s~":
                case "~ls~":
                    return System.ConsoleColor.Gray;

                case "~8~":
                case "~darkgray~":
                case "~ds~":
                    return System.ConsoleColor.DarkGray;

                case "~9~":
                case "~blue~":
                case "~b~":
                case "~lb~":
                    return System.ConsoleColor.Blue;

                case "~10~":
                case "~green~":
                case "~g~":
                case "~lg~":
                    return System.ConsoleColor.Green;

                case "~11~":
                case "~cyan~":
                case "~c~":
                case "~lc~":
                    return System.ConsoleColor.Cyan;

                case "~12~":
                case "~red~":
                case "~r~":
                case "~lr~":
                    return System.ConsoleColor.Red;

                case "~13~":
                case "~magenta~":
                case "~m~":
                case "~lm~":
                    return System.ConsoleColor.Magenta;

                case "~14~":
                case "~yellow~":
                case "~y~":
                case "~ly~":
                    return System.ConsoleColor.Yellow;

                case "~15~":
                case "~white~":
                case "~w~":
                    return System.ConsoleColor.White;

                case "~16~":
                case "~black~":
                case "~k~":
                    return System.ConsoleColor.Black;
            }
        }

        #endregion

        #region Escape

        /// <summary>
        /// Escape string which may contain tilde character
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Escape(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            if (text.IndexOf('~') < 0)
                return text;
            return string.Concat("~`", text.Replace("`", "``"), "`~");
        }

        #endregion

        #region Strip

        /// <summary>
        /// Strip tildes from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strip(string text)
        {
            ColorTextList list = ColorTextList.Explode(text);
            return list.Join("");
        }

        #endregion

        #region Exception

        /// <summary>
        /// Return formatted exception message
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="trace">Write also stack trace</param>
        public static string GetExceptionMessage(Exception exception, bool trace)
        {
            List<string> list = new List<string>();
            string message = (exception.Message ?? "").Trim();
            const string eol = "~0~\r\n";
            if (!String.IsNullOrEmpty(message))
            {
                list.Add(string.Concat("~r~", message));
            }
            if (exception.InnerException != null)
            {
                string next = (exception.InnerException.Message ?? "").Trim();
                if (!String.IsNullOrEmpty(next))
                {
                    list.Add(string.Concat("~m~", next));
                }
            }
            if (trace)
            {
                string pattern = @"^\s+[a-z]+\s+(?<method>[a-z][a-z0-9_\.]*\([^\)\r\n]*\))(?:(?:[\ \t]+[a-z]+[\ \t]+)(?<file>[^\r\n]+))?";
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
                Match m = r.Match(exception.StackTrace);
                while (m.Success)
                {
                    string method = m.Groups["method"].Value;
                    string file = m.Groups["file"].Value;
                    m = m.NextMatch();
                    list.Add(string.Concat(Energy.Core.Tilde.Color.DarkGray
                        , method, " ", Energy.Core.Tilde.Color.Yellow
                        , file).Trim());
                }

                //string comment = (new Regex(@"^\s*\w+\s*", RegexOptions.Multiline).Replace(exception.StackTrace, ""));
                //string[] split = Energy.Base.Text.SplitNewLine(comment);
                //List<string> stack = new List<string>();
                //for (int i = split.Length - 1; i >= 0; i--)
                //{
                //    stack.Add(split[i]);
                //}
                //if (stack.Count > 0)
                //{
                //    message += "~ds~" + String.Join(Environment.NewLine, stack.ToArray()) + "~0~" + Environment.NewLine;
                //}
            }

            message = string.Join(eol, list.ToArray());

            return message;
        }

        /// <summary>
        /// Write out exception message
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="trace">Write also stack trace</param>
        public static void Exception(Exception exception, bool trace)
        {
            string message = GetExceptionMessage(exception, trace);
            if (trace)
                Tilde.WriteLine(message);
            else
                Tilde.Write(message);
        }

        /// <summary>
        /// Write out exception message
        /// </summary>
        /// <param name="exception"></param>
        public static void Exception(Exception exception)
        {
            Exception(exception, false);
        }

        /// <summary>
        /// Pause execution
        /// </summary>
        /// <returns></returns>
        public static string Pause()
        {
            WriteLine(_PauseText);
            string input = System.Console.ReadLine();
            return input;
        }

        /// <summary>
        /// Alias for Exception method.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="trace"></param>
        public static void WriteException(Exception exception, bool trace)
        {
            Exception(exception, trace);
        }

        #endregion

        #region Read

        private static StringBuilder _ReadLineStringBuilder;

        private static readonly object _ReadLineLock = new object();

        /// <summary>
        /// Read line from console if available. Does not wait for user to enter anything
        /// so may be useful in loops. Thread safe. Returns null if user did not press Enter key.
        /// </summary>
        /// <returns></returns>
        public static string ReadLine()
        {
            lock (_ReadLineLock)
            {
                while (System.Console.KeyAvailable)
                {
                    if (_ReadLineStringBuilder == null)
                        _ReadLineStringBuilder = new StringBuilder();
                    System.ConsoleKeyInfo key = System.Console.ReadKey();
                    if (key.Key == System.ConsoleKey.Enter)
                    {
                        string result = _ReadLineStringBuilder.ToString();
                        _ReadLineStringBuilder.Length = 0;
                        Console.WriteLine();
                        return result;
                    }
                    if (key.Key == System.ConsoleKey.Backspace)
                    {
                        if (_ReadLineStringBuilder.Length > 0)
                        {
                            _ReadLineStringBuilder.Length = _ReadLineStringBuilder.Length - 1;
                            Console.Write(" \b");
                        }
                        continue;
                    }
                    _ReadLineStringBuilder.Append(key.KeyChar);
                }
                return null;
            }
        }

        /// <summary>
        /// Read line from console using mask character. Could be useful for passwords.
        /// Returns null if user press ESC key.
        /// </summary>
        /// <returns></returns>
        public static string ReadLine(char? mask)
        {
            StringBuilder stringBuilder = new StringBuilder();
            System.ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return stringBuilder.ToString();
                }
                if (key.Key == System.ConsoleKey.Backspace)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Length = stringBuilder.Length - 1;
                        Console.Write("\b \b");
                    }
                    continue;
                }
                if (!char.IsControl(key.KeyChar))
                {
                    stringBuilder.Append(key.KeyChar);
                    if (mask != null)
                        Console.Write(mask);
                    else
                        Console.Write(key.KeyChar);
                }
            }
            while (key.Key != ConsoleKey.Escape);
            return null;
        }

        #endregion

        #region Input

        public static string Input(string message, string defaultValue)
        {
            if (string.IsNullOrEmpty(message))
            {
            }
            else
            {
                if (message.Contains("{0}"))
                    message = string.Format(message, defaultValue);
            }
            Energy.Core.Tilde.Write(message);
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                input = defaultValue;
            return input;
        }

        public static TInput Input<TInput>(string message, object defaultValue)
        {
            string defaultString = Energy.Base.Cast.ObjectToString(defaultValue);
            string input = Energy.Core.Tilde.Input(message, defaultString);
            return Energy.Base.Cast.StringToObject<TInput>(input);
        }

        #endregion

        #region Length

        /// <summary>
        /// Return total length of tilde string
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        public static int Length(string example)
        {
            return ColorTextList.Explode(example).GetTotalLength();
        }

        #endregion
    }
}