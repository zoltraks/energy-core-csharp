using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextUnique
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] array1 = new string[] { "äußern", "ÄUßERN", "żółty", "ŻÓŁTY", "éphémère", "ÉPHÉMÈRE", };
            Energy.Core.Program.SetConsoleEncoding();
            Energy.Core.Tilde.WriteLine("UTF-8 Console Test: ĄÄĘ...\n");
            Test(array1);
            Energy.Core.Tilde.Pause();
        }

        private static void Test(string[] array)
        {
            Energy.Core.Tilde.WriteLine("~m~Original array ~ds~[ ~y~{0} ~ds~]", string.Join("~g~, ~y~", array));
            array = Energy.Base.Text.Unique(array, true);
            Energy.Core.Tilde.WriteLine("~c~Unique array   ~ds~[ ~y~{0} ~ds~]", string.Join("~g~, ~y~", array));
            Energy.Core.Tilde.Break();
        }
    }
}
