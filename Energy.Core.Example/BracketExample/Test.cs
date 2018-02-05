using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Energy.Base;

namespace BracketExample
{
    class Test
    {
        private static void Check(Bracket bracket, string example)
        {
            Energy.Core.Tilde.WriteLine("Bracket: ~w~{0}", bracket);
            Energy.Core.Tilde.WriteLine("Expression: ~c~{0}", bracket.MatchExpression);
            Console.WriteLine(example);
            int n = 1;
            foreach (string needle in bracket.Find(example))
            {
                Energy.Core.Tilde.WriteLine("~g~{0,2}~dg~. ~ds~[~y~{1}~ds~]"
                    , n, needle);
                n++;
            }
            Console.WriteLine();
        }

        internal static void Angle()
        {
            Energy.Base.Bracket bracket = new Energy.Base.Bracket("<>");
            bracket.Include = "";
            string example = @"Example <parameter1> <parameter2> <p:3> <a><b> <p>>q>";
            Check(bracket, example);
        }

        internal static void Dollar()
        {
            Energy.Base.Bracket bracket = new Energy.Base.Bracket("${", "}");
            bracket.Include = "\\}";
            string example = @"Example $parameter1 ${parameter2} ${a}}} ${a {\} b}";
            Check(bracket, example);
        }

        internal static void Percentage()
        {
            Energy.Base.Bracket bracket = new Energy.Base.Bracket("%");
            string example = @"Example %parameter1% %parameter2%%% %p:3> <a % % % ... end of example";
            Check(bracket, example);
        }
    }
}
