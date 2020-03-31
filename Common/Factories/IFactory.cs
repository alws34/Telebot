using System;
using System.Collections.Generic;

namespace Contracts.Factories
{
    public abstract class IFactory<T>
    {
        protected readonly List<T> _items;

        protected IFactory()
        {
            _items = new List<T>();
        }

        public T Find(Predicate<T> predicate)
        {
            return _items.Find(predicate);
        }

        public IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            return _items.FindAll(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public bool TryGetEntity(Predicate<T> predicate, out T entity)
        {
            entity = _items.Find(predicate);
            return entity != null;
        }
    }
}
