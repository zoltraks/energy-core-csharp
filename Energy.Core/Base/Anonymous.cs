namespace Energy.Base
{
    public class Anonymous
    {
        /// <summary>
        /// Represents parameterless function that returns only string.
        /// </summary>
        /// <returns></returns>
        public delegate string String();

        /// <summary>
        /// Represents function that takes one argument of specified type and returns only string.
        /// </summary>
        /// <typeparam name="TString">Argument type</typeparam>
        /// <returns></returns>
        public delegate string String<TString>(TString argument);

        /// <summary>
        /// Generic function that takes argument of specified type and returns result with the same type.
        /// </summary>
        /// <typeparam name="TState">State type</typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TState State<TState>(TState input);

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        /// <typeparam name="TOut">Type of result object</typeparam>
        public delegate TOut Function<TOut>();

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        /// <typeparam name="TIn">Type of argument for funtion</typeparam>
        /// <typeparam name="TOut">Type of result object</typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TOut Function<TOut, TIn>(TIn input);

        /// <summary>
        /// Generic function delegate.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TParameter1"></typeparam>
        /// <typeparam name="TParameter2"></typeparam>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        public delegate TOut Function<TOut, TParameter1, TParameter2>(TParameter1 parameter1, TParameter2 parameter2);

        /// <summary>
        /// Event function delegate.
        /// </summary>
        public delegate void Event(object self);

        /// <summary>
        /// Event function delegate.
        /// </summary>
        /// <typeparam name="TEvent">Type of argument function will be called with</typeparam>
        /// <param name="argument"></param>
        public delegate void Event<TEvent>(TEvent argument);

        /// <summary>
        /// Generic action delegate.
        /// </summary>
        public delegate void Action();

        /// <summary>
        /// Generic action delegate.
        /// </summary>
        /// <typeparam name="TAction">Type of argument function will be called with</typeparam>
        public delegate void Action<TAction>(TAction argument);

        /// <summary>
        /// Generic action delegate.
        /// </summary>
        /// <typeparam name="TArgument1">Type of first argument</typeparam>
        /// <typeparam name="TArgument2">Type of second argument</typeparam>
        public delegate void Action<TArgument1, TArgument2>(TArgument1 argument1, TArgument2 argument2);
    }
}
