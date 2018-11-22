using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Anonymous
    {
        /// <summary>
        /// Represents function that only output string.
        /// </summary>
        /// <returns></returns>
        public delegate string String();

        /// <summary>
        /// Represents function that changes state.
        /// </summary>
        /// <typeparam name="TState">State type</typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TState State<TState>(TState input);

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TOut Function<TIn, TOut>(TIn input);

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        public delegate void Function<TIn>(TIn input);

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        public delegate void Function();

        /// <summary>
        /// Event function delegate.
        /// </summary>
        public delegate void Event(object self);

        /// <summary>
        /// Event function delegate.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="argument"></param>
        public delegate void Event<TEvent>(TEvent argument);
    }
}
