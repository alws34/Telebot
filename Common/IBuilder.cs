using System.Collections.Generic;

namespace Common
{
    public abstract class IBuilder<T>
    {
        private readonly List<T> _items;

        public IBuilder()
        {
            _items = new List<T>();
        }

        public IBuilder<T> Add(T item)
        {
            _items.Add(item);
            return this;
        }
        public IBuilder<T> AddRange(T[] items)
        {
            _items.AddRange(items);
            return this;
        }
        public T[] Build()
        {
            return _items.ToArray();
        }
    }
}
