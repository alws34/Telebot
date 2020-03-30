using System;

namespace Contracts.Jobs
{
    public abstract class IJob<T> : IJob
    {
        public EventHandler<T> Update;

        protected void RaiseUpdate(T e)
        {
            Update?.Invoke(this, e);
        }
    }

    public abstract class IJob : IFeedback
    {
        public bool Active { get; protected set; }

        public abstract void Start();
        public abstract void Stop();
    }
}
