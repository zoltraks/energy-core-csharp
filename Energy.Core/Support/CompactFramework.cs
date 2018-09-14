#if CFNET
    //
#elif WindowsCE || PocketPC || WINDOWS_PHONE
    //
#define CFNET
#elif COMPACT_FRAMEWORK
    //
#define CFNET
#else
    //
#endif

using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Support
{
    public class CompactFramework
    {
    }
}

#if CFNET

namespace System
{
    //
    // Summary:
    //     Specifies constants that define foreground and background colors for the console.
    public enum ConsoleColor
    {
        //
        // Summary:
        //     The color black.
        Black = 0,
        //
        // Summary:
        //     The color dark blue.
        DarkBlue = 1,
        //
        // Summary:
        //     The color dark green.
        DarkGreen = 2,
        //
        // Summary:
        //     The color dark cyan (dark blue-green).
        DarkCyan = 3,
        //
        // Summary:
        //     The color dark red.
        DarkRed = 4,
        //
        // Summary:
        //     The color dark magenta (dark purplish-red).
        DarkMagenta = 5,
        //
        // Summary:
        //     The color dark yellow (ochre).
        DarkYellow = 6,
        //
        // Summary:
        //     The color gray.
        Gray = 7,
        //
        // Summary:
        //     The color dark gray.
        DarkGray = 8,
        //
        // Summary:
        //     The color blue.
        Blue = 9,
        //
        // Summary:
        //     The color green.
        Green = 10,
        //
        // Summary:
        //     The color cyan (blue-green).
        Cyan = 11,
        //
        // Summary:
        //     The color red.
        Red = 12,
        //
        // Summary:
        //     The color magenta (purplish-red).
        Magenta = 13,
        //
        // Summary:
        //     The color yellow.
        Yellow = 14,
        //
        // Summary:
        //     The color white.
        White = 15
    }
}

#endif
