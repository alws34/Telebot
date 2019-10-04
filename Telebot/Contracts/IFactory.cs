using System;

namespace Telebot.Contracts
{
    public interface IFactory<T>
    {
        T FindEntity(Predicate<T> predicate);
        T[] GetAllEntities();
    }
}
