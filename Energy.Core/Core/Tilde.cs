using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Energy.Enumeration;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Simple elegant text coloring engine for console programs.
    /// Based on Empty Page #0 color engine.
    /// Color may be changed by value surrounded by tilde ~ character.
    /// One tilde followed by other than letter or number or 
    /// more than one tilde chars is left as is.
    /// </summary>
    /// <remarks>
    /// List of available colors:
    /// <br/>
    /// <table>
    /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>               <td>Default color</td></tr>
    /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>           <td>Dark blue</td></tr>
    /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>           <td>Dark green</td></tr>        
    /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>           <td>Dark cyan</td></tr>
    /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>           <td>Dark red</td></tr>
    /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>           <td>Dark magenta</td></tr>
    /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>           <td>Dark yellow</td></tr>
    /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~<br/>~ls~</td>   <td>Gray / Silver</td></tr>
    /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>           <td>Dark gray</td></tr>
    /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>            <td>Black</td></tr>
    /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>            <td>Black</td></tr>
    /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>            <td>Cyan</td></tr>
    /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>            <td>Red</td></tr>
    /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>            <td>Magenta</td></tr>
    /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>            <td>Yellow</td></tr>
    /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>            <td>White</td></tr>
    /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>            <td>Black / Carbon</td></tr>
    /// </table>
    /// </remarks>
    public static class Tilde
    {
        #region Class

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
                public ConsoleColor? Color;

                /// <summary>
                /// Text
                /// </summary>
                public string Text;
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

        private static bool _NoColor;
        /// <summary>Disable console colors</summary>
        public static bool NoColor
        {
            get
            {
                return _NoColor;
            }
            set
            {
                if (value == _NoColor)
                    return;
                SetNoColor(value);
            }
        }

        public static void SetNoColor(bool value)
        {
            _NoColor = value;
        }

        private static string _PauseText = "Enter ~w~anything~0~ to ~y~continue~0~...";
        /// <summary>DefaultPauseText</summary>
        public static string PauseText { get { return _PauseText; } set { _PauseText = value; } }

        private static string _ExampleColorPalleteTildeString = ""
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
            + "~black~ ~ black ~  ~ 16 ~    "
            ;
        /// <summary>
        /// Cheat sheet for all colors defined by default
        /// </summary>
        public static string ExampleColorPalleteTildeString { get { return _ExampleColorPalleteTildeString; } set { _ExampleColorPalleteTildeString = value; } }

        #endregion

        #region Explode

        /// <summary>
        /// Explode text to a list separated by color change
        /// </summary>
        /// <param name="message">Text</param>
        /// <returns>List of texts</returns>
        public static TextList Explode(string message)
        {
            if (message == null)
                return null;
            TextList list = new TextList();
            Match m = Regex.Match(message, @"~\d+~|~[\w\d]+~|~+|[^~]+");
            System.ConsoleColor? current = null;
            while (m.Success)
            {
                if (m.Value.StartsWith("~") && m.Value.EndsWith("~") && m.Value.Length > 2)
                {
                    current = TildeColorToConsoleColor(m.Value);
                }
                else
                {
                    list.Add(new TextList.Item() { Color = current, Text = m.Value });
                }
                m = m.NextMatch();
            }
            return list;
        }

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
            Tilde.Write("~15~" + question + (String.IsNullOrEmpty(value) ? "" : "~13~" + " [ " + "~9~" + value + "~13~" + " ]") + "~0~" + " : ");
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
            string text = Energy.Cast.As.String(value);
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
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray</para>
        /// <para>~9~, ~blue~, ~b~, Black</para>
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
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>               <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>           <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>           <td>Dark green</td></tr>        
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>           <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>           <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>           <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>           <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~<br/>~ls~</td>   <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>           <td>Dark gray</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>            <td>Black</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>            <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>            <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>            <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>            <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>            <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>            <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>            <td>Black / Carbon</td></tr>
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
            TextList list = Explode(value);
            if (list.Count == 0)
                return;
            lock (_ConsoleLock)
            {
                // TODO Fix new lines :)
                // TODO Optionally wrap words?
                // TODO Check security
                System.ConsoleColor previousForegroundColor = System.Console.ForegroundColor;
                System.ConsoleColor defaultForegroundColor = _Foreground != null ? (ConsoleColor)_Foreground : previousForegroundColor;                
                for (int i = 0; i < list.Count; i++)
                {
                    System.ConsoleColor foregroundColor = list[i].Color != null ? (ConsoleColor)list[i].Color : defaultForegroundColor;
                    System.Console.ForegroundColor = foregroundColor;
                    System.Console.Write(list[i].Text);
                }
                System.Console.ForegroundColor = previousForegroundColor;
            }
        }

        /// <summary>
        /// <para>
        /// Write color text with line ending containing modifiers like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
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
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray</para>
        /// <para>~9~, ~blue~, ~b~, Black</para>
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
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>               <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>           <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>           <td>Dark green</td></tr>        
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>           <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>           <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>           <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>           <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~<br/>~ls~</td>   <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>           <td>Dark gray</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>            <td>Black</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>            <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>            <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>            <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>            <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>            <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>            <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>            <td>Black / Carbon</td></tr>
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
        /// Write color text with line ending containing modifiers like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
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
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray</para>
        /// <para>~9~, ~blue~, ~b~, Black</para>
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
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>               <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>           <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>           <td>Dark green</td></tr>        
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>           <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>           <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>           <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>           <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~<br/>~ls~</td>   <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>           <td>Dark gray</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>            <td>Black</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>            <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>            <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>            <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>            <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>            <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>            <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>            <td>Black / Carbon</td></tr>
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
        /// Write color text with line ending containing modifiers like ~7~, ~s~ , ~12~, ~r~,  ~10~, ~g~, ~9~ , ~b~ , ~15~, ~w~, ~14~, ~y~, ~13~, ~m~, ...
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
        /// <para>~8~, ~darkgray~, ~ds~, Dark gray</para>
        /// <para>~9~, ~blue~, ~b~, Black</para>
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
        /// <tr><td>~0~</td>    <td>~default~</td>      <td></td>               <td>Default color</td></tr>
        /// <tr><td>~1~</td>    <td>~darkblue~</td>     <td>~db~</td>           <td>Dark blue</td></tr>
        /// <tr><td>~2~</td>    <td>~darkgreen~</td>    <td>~dg~</td>           <td>Dark green</td></tr>        
        /// <tr><td>~3~</td>    <td>~darkcyan~</td>     <td>~dc~</td>           <td>Dark cyan</td></tr>
        /// <tr><td>~4~</td>    <td>~darkred~</td>      <td>~dr~</td>           <td>Dark red</td></tr>
        /// <tr><td>~5~</td>    <td>~darkmagenta~</td>  <td>~dm~</td>           <td>Dark magenta</td></tr>
        /// <tr><td>~6~</td>    <td>~darkyellow~</td>   <td>~dy~</td>           <td>Dark yellow</td></tr>
        /// <tr><td>~7~</td>    <td>~gray~</td>         <td>~s~<br/>~ls~</td>   <td>Gray / Silver</td></tr>
        /// <tr><td>~8~</td>    <td>~darkgray~</td>     <td>~ds~</td>           <td>Dark gray</td></tr>
        /// <tr><td>~9~</td>    <td>~blue~</td>         <td>~b~</td>            <td>Black</td></tr>
        /// <tr><td>~10~</td>   <td>~green~</td>        <td>~g~</td>            <td>Black</td></tr>
        /// <tr><td>~11~</td>   <td>~cyan~</td>         <td>~c~</td>            <td>Cyan</td></tr>
        /// <tr><td>~12~</td>   <td>~red~</td>          <td>~r~</td>            <td>Red</td></tr>
        /// <tr><td>~13~</td>   <td>~magenta~</td>      <td>~m~</td>            <td>Magenta</td></tr>
        /// <tr><td>~14~</td>   <td>~yellow~</td>       <td>~y~</td>            <td>Yellow</td></tr>
        /// <tr><td>~15~</td>   <td>~white~</td>        <td>~w~</td>            <td>White</td></tr>
        /// <tr><td>~16~</td>   <td>~black~</td>        <td>~k~</td>            <td>Black / Carbon</td></tr>
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
            public static string green = "~10~";
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

        public static string Escape(string text)
        {
            return text;
        }

        #endregion

        #region Strip

        public static string Strip(string text)
        {
            return text;
        }

        #endregion

        #region Exception

        /// <summary>
        /// Write out exception message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="trace">Write also stack trace</param>
        public static void Exception(Exception exception, bool trace)
        {
            string message = (exception.Message ?? "").Trim();
            if (!String.IsNullOrEmpty(message))
            {
                message = "~r~" + message + "~0~\r\n";
            }
            if (exception.InnerException != null)
            {
                string next = (exception.InnerException.Message ?? "").Trim();
                if (!String.IsNullOrEmpty(next))
                {
                    message += "~m~" + next + "~0~\r\n";
                }
            }
            if (trace)
            {
                string comment = (new Regex(@"^\s*\w+\s*", RegexOptions.Multiline).Replace(exception.StackTrace, ""));
                string[] split = Energy.Base.Text.SplitNewLine(comment);
                List<string> stack = new List<string>();
                for (int i = split.Length - 1; i >= 0; i--)
                {
                    stack.Add(split[i]);
                }
                if (stack.Count > 0)
                {
                    message += "~ds~" + String.Join(Environment.NewLine, stack.ToArray()) + "~0~" + Environment.NewLine;
                }
            }
            Tilde.Write(message);
        }

        /// <summary>
        /// Write out exception message.
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
    }
}