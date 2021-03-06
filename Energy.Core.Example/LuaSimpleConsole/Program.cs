﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaSimpleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Print.Welcome();

            using (Neo.IronLua.Lua lua = new Neo.IronLua.Lua())
            {
                var state = lua.CreateEnvironment();

                string name = "test.lua";
                state.DoChunk("message = 'Hello World!'", name); // create a variable in lua
                state.DoChunk("message = 'Hello World 2!'", name); // modify a variable in lua
                state.DoChunk(@"
x = 1
y = 2
x, y = y, x
print(x .. y)
"
                    , name); // modify a variable in lua
                Console.WriteLine(state["message"]); // access a variable in c#
                Console.WriteLine(state["x"]); // access a variable in c#
                Console.WriteLine(state["y"]); // access a variable in c#
            }

            Console.ReadLine();
        }
    }
}
