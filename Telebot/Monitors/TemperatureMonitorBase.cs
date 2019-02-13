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

        public bool IsActive => timer.Enabled;

        protected readonly IEnumerable<ITemperatureProvider> temperatureProviders;

        protected Action<IEnumerable<HardwareInfo>> callback;

        public TemperatureMonitorBase()
        {
            timer = new Timer();
            temperatureProviders = Program.container.GetAllInstances<ITemperatureProvider>();
        }

        public void Start(Action<IEnumerable<HardwareInfo>> callback)
        {
            this.callback = callback;
            timer.Start();
        }

        public virtual void Start(TimeSpan duration, TimeSpan interval, Action<IEnumerable<HardwareInfo>> callback)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
