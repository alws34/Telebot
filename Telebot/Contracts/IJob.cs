using Common;
using System;
using Telebot.Common;

namespace Telebot.Contracts
{
    public abstract class IJob<T> : IFeedback
    {
        public event EventHandler<T> Update;

        protected void RaiseUpdate(T e)
        {
            Update?.Invoke(this, e);
        }

        public JobType JobType { get; protected set; }
        public bool Active { get; protected set; }

        public abstract void Start();
        public abstract void Stop();
    }
}
