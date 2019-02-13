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

        public event EventHandler<IEnumerable<HardwareInfo>> TemperatureChanged;

        protected void OnTemperatureChanged(IEnumerable<HardwareInfo> info)
        {
            TemperatureChanged?.Invoke(this, info);
        }

        public TemperatureMonitorBase()
        {
            timer = new Timer();
            temperatureProviders = Program.container.GetAllInstances<ITemperatureProvider>();
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
