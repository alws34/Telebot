using System;
using System.Collections.Generic;
using Telebot.Contracts;

namespace Telebot.Common
{
    public abstract class Factory<T> : IFactory<T>
    {
        protected readonly List<T> _items;

        public Factory()
        {
            _items = new List<T>();
        }

        public T FindEntity(Predicate<T> predicate)
        {
            return _items.Find(x => predicate(x));
        }

        public T[] GetAllEntities()
        {
            return _items.ToArray();
        }

        public bool TryGetEntity(Predicate<T> predicate, out T entity)
        {
            entity = _items.Find(x => predicate(x));
            return entity != null;
        }
    }
}
