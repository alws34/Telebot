using System;

namespace Contracts
{
    public abstract class IApi
    {
        protected Action Action;

        public void Invoke()
        {
            Action();
        }
    }

    public abstract class IApi<T>
    {
        protected Func<T> Func;

        public void Invoke(Action<T> callback)
        {
            callback(Func.Invoke());
        }
    }
}
