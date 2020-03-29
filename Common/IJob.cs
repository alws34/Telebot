using Enums;
using System;

namespace Contracts
{
    public abstract class IJob<T> : IJob
    {
        public event EventHandler<T> Update;

        protected void RaiseUpdate(T e)
        {
            Update?.Invoke(this, e);
        }
    }

    public abstract class IJob : IFeedback
    {
        public JobType JobType { get; protected set; }
        public bool Active { get; protected set; }

        public abstract void Start();
        public abstract void Stop();
    }
}
