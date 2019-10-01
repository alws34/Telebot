using CPUID.Contracts;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Telebot.Temperature
{
    public abstract class TempMonBase : ITempMon
    {
        protected readonly Timer timer;

        public bool IsActive
        {
            get => timer.Enabled;
            protected set => timer.Enabled = value;
        }

        protected readonly List<IDevice> devices;

        public event EventHandler<TempChangedArgs> TemperatureChanged;

        protected void RaiseTemperatureChanged(TempChangedArgs e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        protected TempMonBase()
        {
            timer = new Timer();
            devices = new List<IDevice>();
        }

        public virtual void Start()
        {
            timer.Start();
        }

        public virtual void Start(TimeSpan duration, TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
