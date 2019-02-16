using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.Monitors
{
    public abstract class TemperatureMonitorBase : ITemperatureMonitor
    {
        protected readonly Timer timer;

        public bool IsActive
        {
            get { return timer.Enabled; }
            protected set { timer.Enabled = value; }
        }

        protected readonly IEnumerable<IHardwareProvider> temperatureProviders;

        public event EventHandler<IEnumerable<HardwareInfo>> TemperatureChanged;

        protected void OnTemperatureChanged(IEnumerable<HardwareInfo> e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        public TemperatureMonitorBase()
        {
            timer = new Timer();
            temperatureProviders = Program.container.GetAllInstances<IHardwareProvider>();
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
