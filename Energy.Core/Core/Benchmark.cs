using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;

namespace Energy.Core
{
    public class Benchmark
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
            /// Number format
            /// </summary>
            public string Format = "0.000";

            /// <summary>
            /// Represent result as string
            /// </summary>
            /// <param name="verbose">Include additional information</param>
            /// <param name="format">Number format to use</param>
            /// <returns></returns>
            public string ToString(bool verbose, string format)
            {
                if (string.IsNullOrEmpty(format))
                    format = Format;
                List<string> list = new List<string>();
                if (verbose && Garbage > 0)
                {
                    list.Add(string.Concat("Garbage collected in ", Garbage.ToString(format, CultureInfo.InvariantCulture), " s"));
                }
                if (Time > 0 && (verbose || Average == 0))
                {
                    string text = "Time taken";
                    if (!string.IsNullOrEmpty(Name)) text += " by " + Name;
                    if (Iterations > 1) text += " in " + Iterations.ToString() + " iterations";
                    text += string.Concat(" ", Time.ToString(format, CultureInfo.InvariantCulture), " s");
                    list.Add(text);
                }
                if (Average > 0)
                {
                    list.Add(string.Concat("Average time of execution ", Average.ToString(format, CultureInfo.InvariantCulture), " s"));
                }
                return String.Join(Environment.NewLine, list.ToArray());
            }

            /// <summary>
            /// Represent result as string
            /// </summary>
            /// <param name="verbose">Include additional information</param>
            /// <returns></returns>
            public string ToString(bool verbose)
            {
                return ToString(verbose, Format);
            }

            /// <summary>
            /// Represent result as string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return ToString(true, Format);
            }
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        /// <param name="name"></param>
        public static Result Profile(Action function, int iterations, string name)
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

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        public static Result Profile(Action function, int iterations)
        {
            return Profile(function, iterations, null);
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        public static Result Profile(Action function)
        {
            return Profile(function, 1, null);
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