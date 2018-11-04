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
            throw new NotImplementedException();
        }

        public bool Setup(string[] args)
        {
            Energy.Core.Log.Logger logger = new Energy.Core.Log.Logger();

            string path = Energy.Core.Application.GetExecutionPath(System.Reflection.Assembly.GetExecutingAssembly());

            logger.Destination.Add(new Energy.Core.Log.Target.File()
            {
                Path = path,
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
