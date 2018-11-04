using System;
using System.Collections.Generic;
using System.Text;

namespace LogFileThreadStress
{
    public class App : Energy.Interface.ICommandProgram
    {
        public bool Initialize(string[] args)
        {
            throw new NotImplementedException();
        }

        public bool Run(string[] args)
        {
            Energy.Core.Tilde.Pause();
            return true;
        }

        public bool Setup(string[] args)
        {
            Energy.Core.Bug.Log = Energy.Core.Log.Default;

            Energy.Core.Log.Logger logger = new Energy.Core.Log.Logger();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string executionPath = Energy.Core.Application.GetExecutionPath(assembly);
            Energy.Core.Log.Default.Setup(System.IO.Path.ChangeExtension(assembly.Location, ".log"), true, null);

            logger.Destination.Add(new Energy.Core.Log.Target.File()
            {
                Path = System.IO.Path.Combine(executionPath, "New.log"),
                Immediate = true,
                Background = true,
            });

            logger.Destination.Add(new Energy.Core.Log.Target.Console()
            {
                Immediate = true,
                Background = true,
            });

            logger.Write("SETUP");

            return true;
        }
    }
}
