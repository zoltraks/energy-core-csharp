using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaSimpleConsole
{
    public static class Print
    {
        public static void Welcome()
        {
            Energy.Core.Tilde.WriteLine("~w~Welcome to ~c~Lua Simple Console ~w~ program. ~s~Enter \"~g~h~s~\" to print help message.");
        }

        public static void Help()
        {

        }
    }
}
