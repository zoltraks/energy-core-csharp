namespace Energy.Base
{
    public class Range<T>
    {
        public class Array : System.Collections.Generic.List<T>
        {
        }

        public T Minimum;
        public T Maximum;
    }
}
