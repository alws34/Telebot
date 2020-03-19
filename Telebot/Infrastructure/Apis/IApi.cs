using System;

namespace Telebot.Infrastructure.Apis
{
    public abstract class IApi
    {
        public Action Action { get; protected set; }

        public void Invoke()
        {
            Action();
        }
    }

    public abstract class IApi<T>
    {
        public Func<T> Func { get; protected set; }

        public void Invoke(Action<T> callback)
        {
            callback(Func.Invoke());
        }
    }
}
