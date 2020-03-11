using System;

namespace Telebot.Contracts
{
    public interface IFactory<T>
    {
        T FindEntity(Predicate<T> predicate);
        bool TryGetEntity(Predicate<T> predicate, out T entity);
        T[] GetAllEntities();
    }
}
