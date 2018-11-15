namespace Energy.Interface
{
    public interface IStack<T>
    {
        void Push(T item);

        T Pull();
    }
}