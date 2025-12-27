using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Energy.Base.Log;
using System.Linq;

namespace RestBinaryGetPut
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Energy.Core.Program.SetConsoleEncoding();
                Test.CheckGet("http://www.google.com");
                Test.AspNetCoreApi("http://localhost:16000/api/");
                //Test.S();
                //Test.Make();
            }
            catch (Exception x)
            {
                Energy.Core.Tilde.Exception(x, true);
            }
            finally
            {
                Energy.Core.Tilde.Pause();
            }
        }
    }
}
