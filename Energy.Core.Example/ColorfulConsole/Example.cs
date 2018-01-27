using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ColorfulConsole
{
    public class Example
    {
        internal static void TwoColor()
        {
            string dream = "a dream of {0} and {1} and {2} and {3} and {4} and {5} and {6} and {7} and {8} and {9}...";
            string[] fruits = new string[]
            {
                "bananas",
                "strawberries",
                "mangoes",
                "pineapples",
                "cherries",
                "oranges",
                "apples",
                "peaches",
                "plums",
                "melons"
            };

            Colorful.Console.WriteLineFormatted(dream, Color.LightGoldenrodYellow, Color.Gray, fruits);
        }

        internal static void Reset()
        {
            Colorful.Console.Clear();
            Colorful.Console.ResetColor();
            Colorful.Console.Clear();
        }

        internal static void StoryFragment()
        {
            string[] storyFragments = new string[]
            {
                "Some say the world will end in fire,",
                "Some say in ice.",
                "From what I’ve tasted of desire",
                "I hold with those who favor fire.",
                "But if it had to perish twice,",
                "I think I know enough of hate",
                "To say that for destruction ice",
                "Is also great",
                "And would suffice.",
                "Author: Robert Frost"
            };
            int r = 225;
            int g = 255;
            int b = 250;
            for (int i = 0; i < 10; i++)
            {
                Colorful.Console.WriteLine(storyFragments[i], Color.FromArgb(r, g, b));

                r -= 18;
                b -= 9;
            }
            Colorful.Console.ResetColor();
        }

        internal static void WithGradient()
        {
            List<char> chars = new List<char>()
            {
                'r', 'e', 'x', 's', 'z', 'q', 'j', 'w', 't', 'a', 'b', 'c', 'l', 'm',
                'r', 'e', 'x', 's', 'z', 'q', 'j', 'w', 't', 'a', 'b', 'c', 'l', 'm',
                'r', 'e', 'x', 's', 'z', 'q', 'j', 'w', 't', 'a', 'b', 'c', 'l', 'm',
                'r', 'e', 'x', 's', 'z', 'q', 'j', 'w', 't', 'a', 'b', 'c', 'l', 'm'
            };
            Colorful.Console.WriteWithGradient(chars, Color.Yellow, Color.Fuchsia, 12);
        }

        internal static void SeveralColors()
        {
            string dream = "a dream of {0} and {1} and {2} and {3} and {4} and {5} and {6} and {7} and {8} and {9}...";
            Colorful.Formatter[] fruits = new Colorful.Formatter[]
            {
                new Colorful.Formatter("bananas", Color.LightGoldenrodYellow),
                new Colorful.Formatter("strawberries", Color.Pink),
                new Colorful.Formatter("mangoes", Color.PeachPuff),
                new Colorful.Formatter("pineapples", Color.Yellow),
                new Colorful.Formatter("cherries", Color.Red),
                new Colorful.Formatter("oranges", Color.Orange),
                new Colorful.Formatter("apples", Color.LawnGreen),
                new Colorful.Formatter("peaches", Color.MistyRose),
                new Colorful.Formatter("plums", Color.Indigo),
                new Colorful.Formatter("melons", Color.LightGreen),
            };

            Colorful.Console.WriteLineFormatted(dream, Color.Gray, fruits);
        }
    }
}
