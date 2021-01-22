using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public static class Hello
    {
        public static string World()
        {
            return "Hello, World.";
        }

        public static string World(char exclamation)
        {
            return "Hello, World" + exclamation.ToString();
        }
    }
}
