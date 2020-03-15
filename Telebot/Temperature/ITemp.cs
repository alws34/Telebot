using Common;
using CPUID.Base;
using System;
using System.Collections.Generic;
using Telebot.Common;
using Telebot.Contracts;

namespace Telebot.Temperature
{
    public abstract class ITemp : INotifyable, IJob<TempArgs>
    {
        public JobType JobType { get; protected set; }

        public bool IsActive { get; protected set; }

        protected readonly List<IDevice> devices;

        public event EventHandler<TempArgs> Update;

        protected ITemp()
        {
            devices = new List<IDevice>();
        }

        protected void RaiseUpdate(TempArgs e)
        {
            Update?.Invoke(this, e);
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
