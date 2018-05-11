using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;

namespace Energy.Core
{
    public class Benchmark
    {
        #region Action

        /// <summary>
        /// Test function delegate
        /// </summary>
        public delegate void Action();

        #endregion

        #region Result

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

        #endregion

        #region Profile

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        /// <param name="warm"></param>
        public static Result Profile(Action function, int iterations, int warm)
        {
            Result result = new Result() { Name = function.Method.Name, Iterations = iterations };

            // warm up //

            while (warm-- > 0)
            {
                function();
                iterations--;
            }

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
        /// <param name="name"></param>
        public static Result Profile(Action function, int iterations, string name)
        {
            Result result = Profile(function, iterations, 0);
            if (!string.IsNullOrEmpty(name))
                result.Name = name;
            return result;
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        /// <param name="warm"></param>
        /// <param name="name"></param>
        public static Result Profile(Action function, int iterations, int warm, string name)
        {
            Result result = Profile(function, iterations, warm);
            if (!string.IsNullOrEmpty(name))
                result.Name = name;
            return result;
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="iterations"></param>
        public static Result Profile(Action function, int iterations)
        {
            return Profile(function, iterations, 0);
        }

        /// <summary>
        /// Profile function
        /// </summary>
        /// <param name="function"></param>
        public static Result Profile(Action function)
        {
            return Profile(function, 1, 0);
        }

        #endregion

        #region TimeMeasurement

        public class TimeMeasurement: IDisposable
        {
            public static class Default
            {
                public static string OutputFormat = "{0}";

                public static int TimePrecision = 6;
            }

            public bool Console;

            public string Format;

            public DateTime Begin;

            public DateTime Finish;

            public int TimePrecision;

            public TimeMeasurement()
            {
                TimePrecision = -1;
            }

            public TimeMeasurement(bool console)
                : this()
            {
                Console = console;
            }

            public TimeMeasurement(bool console, string format)
                : this(console)
            {
                Format = format;
            }

            public void Dispose()
            {
                Stop();

                string format = Format;
                if (string.IsNullOrEmpty(format))
                    format = Default.OutputFormat;
                TimeSpan span = Finish - Begin;
                string timeString = Energy.Base.Cast.DoubleToString(span.TotalSeconds, TimePrecision);
                string message = string.Format(format, timeString);
                
                if (Console)
                {
                    System.Console.WriteLine(message);
                }

                Energy.Core.Bug.Write(message);
            }

            public TimeMeasurement Start()
            {
                Begin = DateTime.Now;
                Finish = DateTime.MinValue;
                return this;
            }

            public TimeMeasurement Stop()
            {
                if (Finish == DateTime.MinValue)
                    Finish = DateTime.Now;
                return this;
            }
        }

        #endregion

        #region Time

        public static TimeMeasurement Time(bool console, string format)
        {
            return (new TimeMeasurement(console, format)).Start();
        }

        public static TimeMeasurement Time(bool console)
        {
            return (new TimeMeasurement(console)).Start();
        }

        public static TimeMeasurement Time()
        {
            return (new TimeMeasurement()).Start();
        }

        #endregion
    }
}