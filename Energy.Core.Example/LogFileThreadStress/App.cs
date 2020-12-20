using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LogFileThreadStress
{
    public class App : Energy.Interface.ICommandProgram
    {
        private Energy.Core.Log.Logger Logger;

        public bool Initialize(string[] args)
        {
            Energy.Core.Bug.ExceptionTrace = true;

            return true;
        }

        public bool Run(string[] args)
        {
            this.Logger.Write("RUN");

            int threadCount = 20;

            for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
            {
                new Thread(() => { WriteSomething(threadIndex); })
                {
                    IsBackground = true,
                }.Start();
                Thread.Sleep(100);
            }

            Energy.Core.Tilde.Pause();

            return true;
        }

        private void WriteSomething(int threadIndex)
        {
            int n = Energy.Base.Random.GetNextInteger(10, 50);
            int t = Energy.Base.Random.GetNextInteger(1, 15);
            for (int i = 0; i < n; i++)
            {
                Logger.Write(""
                    + threadIndex.ToString().PadRight(4, ' ')
                    + n.ToString().PadRight(4, ' ')
                    + t.ToString().PadRight(4, ' ')
                    + i.ToString().PadRight(500, '.')
                    );
                if (t > 0)
                    Thread.Sleep(t);
            }
        }

        public bool Setup(string[] args)
        {
            Energy.Core.Bug.Logger = Energy.Core.Log.Default;

            Energy.Core.Bug.ExceptionTrace = false;

            Energy.Core.Log.Logger logger = new Energy.Core.Log.Logger();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string executionPath = Energy.Core.Program.GetExecutionDirectory();
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
                Enable = false,
            });

            logger.Write("SETUP");

            this.Logger = logger;

            return true;
        }
    }
}
