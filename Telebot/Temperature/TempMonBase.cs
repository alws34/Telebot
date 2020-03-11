using CPUID.Base;
using System;
using System.Collections.Generic;
using Telebot.Common;
using Telebot.Contracts;

namespace Telebot.Temperature
{
    public abstract class TempMonBase : IJob<TempChangedArgs>
    {
        public JobType JobType { get; protected set; }

        public bool IsActive { get; protected set; }

        protected readonly List<IDevice> devices;

        public event EventHandler<TempChangedArgs> Update;

        protected void RaiseTemperatureChanged(TempChangedArgs e)
        {
            Update?.Invoke(this, e);
        }

        protected TempMonBase()
        {
            devices = new List<IDevice>();
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
