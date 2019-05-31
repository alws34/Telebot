using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;

namespace Telebot.Temperature
{
    public abstract class TemperatureMonitorBase : ITemperatureMonitor
    {
        protected readonly Timer timer;

        public bool IsActive
        {
            get { return timer.Enabled; }
            protected set { timer.Enabled = value; }
        }

        protected readonly IEnumerable<IDeviceProvider> deviceProviders;

        public event EventHandler<IEnumerable<IDeviceProvider>> TemperatureChanged;

        protected void OnTemperatureChanged(IEnumerable<IDeviceProvider> e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        protected TemperatureMonitorBase()
        {
            timer = new Timer();

            deviceProviders = Program.container.GetAllInstances<IDeviceProvider>();
        }

        public void Start()
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
