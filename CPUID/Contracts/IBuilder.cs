namespace CPUID.Contracts
{
    public interface IBuilder<T>
    {
        IBuilder<T> Add(T item);
        IBuilder<T> AddRange(T[] items);
        T[] Build();
    }
}
