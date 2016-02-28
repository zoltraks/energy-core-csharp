using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Energy.Core
{
    public class Profiler
    {
        /// <summary>
        /// Test function delegate
        /// </summary>
        public delegate void Action();

        public class Result
        {
            /// <summary>
            /// Name of task
            /// </summary>
            public string Name;

            /// <summary>
            /// Number of iterations
            /// </summary>
            public int Iterations;

            /// <summary>
            /// Time taken by GC
            /// </summary>
            public double Garbage;

            /// <summary>
            /// Total time taken in seconds
            /// </summary>
            public double Time;

            /// <summary>
            /// Average time of execution
            /// </summary>
            public double Average;

            /// <summary>
            /// Represent as string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                List<string> list = new List<string>();
                if (Garbage > 0)
                {
                    list.Add(String.Format("Garbage collected in {0:0.000} s", Garbage));
                }
                if (Time > 0)
                {
                    string text = "Time taken";
                    if (!String.IsNullOrEmpty(Name)) text += " by " + Name;
                    if (Iterations > 1) text += " in " + Iterations.ToString() + " iterations";
                    text += String.Format(" {0:0.000} s", Time);
                    list.Add(text);
                }
                if (Average > 0)
                {
                    list.Add(String.Format("Average time of execution {0:0.000} s", Average));
                }
                return String.Join(Environment.NewLine, list.ToArray());
            }
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        /// <param name="name"></param>
        public static Result Profile(Action function, int iterations = 1, string name = null)
        {
            Result result = new Result() { Name = name ?? function.Method.Name, Iterations = iterations };

            // warm up //

            function();

            Stopwatch watch = new Stopwatch();

            watch.Start();

            // clean up //

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Stop();

            result.Garbage = watch.Elapsed.TotalMilliseconds / 1000;

            // go on //

            watch.Reset();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                function();
            }
            watch.Stop();

            result.Time = watch.Elapsed.TotalMilliseconds / 1000;

            if (iterations > 1)
            {
                result.Average = watch.Elapsed.TotalMilliseconds / iterations / 1000;
            }

            return result;
        }

        [System.AttributeUsage(System.AttributeTargets.Parameter)]
        public class Description : System.Attribute
        {
            private string description;
            public double version;

            public Description(string description)
            {
                this.description = description;
            }
        }
    }
}