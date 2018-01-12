using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaExampleNLua
{
    class Program
    {
        static void Main(string[] args)
        {
            NLua.Lua state = new NLua.Lua();
            state.DoString(@"
    function ScriptFunc (val1, val2)
        if val1 > val2 then
            return val1 + 1
        else
            return val2 - 1
        end
    end
    ");
            var scriptFunc = state["ScriptFunc"] as NLua.LuaFunction;
            var res = scriptFunc.Call(3, 5).First();
            // LuaFunction.Call will also return a array of objects, since a Lua function
            // can return multiple values
            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}
