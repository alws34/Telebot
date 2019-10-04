using CPUID.Contracts;
using System;
using System.Collections.Generic;

namespace Telebot.Temperature
{
    public abstract class TempMonBase : ITempMon
    {
        public TempMonType TempMonType { get; protected set; }

        public bool IsActive { get; protected set; }

        protected readonly List<IDevice> devices;

        public event EventHandler<TempChangedArgs> TemperatureChanged;

        protected void RaiseTemperatureChanged(TempChangedArgs e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        protected TempMonBase()
        {
            devices = new List<IDevice>();
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
