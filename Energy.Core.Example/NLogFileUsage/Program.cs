using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLogFileUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            Energy.Core.Tilde.WriteLine("Welcome ~ ~.~~~~ to ~lr~Energy.Core.Log ~0~and ~lg~NLog ~0~example.");
            Energy.Core.Log l = new Energy.Core.Log();
            //l.Write()
            l.Write((Energy.Base.Log.Entry)"Custom message 1");
            Energy.Core.Log.Target.Console.Default.Immediate = false;
            Energy.Core.Log.Target.Console.Default.Color = true;
            Energy.Base.Log.Entry.IsToStringWide = true;
            // this will clean previously added "Custom message 1"
            l.Clean();
            l.Destination.Add(Energy.Core.Log.Target.Console.Default);
            l.Write((Energy.Base.Log.Entry)"Custom message 2");
            l.Clean();
            Energy.Core.Log.Target.Console.Default.Immediate = true;
            l.Write((Energy.Base.Log.Entry)"Next message");
            l.Flush();
            Console.ReadLine();
        }
    }
}
