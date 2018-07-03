using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Anonymous
    {
        /// <summary>
        /// Represents function that only output string
        /// </summary>
        /// <returns></returns>
        public delegate string String();

        public delegate TOut Function<TIn, TOut>(TIn input);

        public delegate void Function<TIn>(TIn input);

        public delegate void Function();

        public delegate void Event();

        public delegate void Event<TEvent>(TEvent argument);
    }
}
