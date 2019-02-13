using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Managers;
using Telebot.Models;

namespace Telebot.Monitors
{
    public class PermanentTempMonitor : TemperatureMonitorBase
    {
        private readonly ISettings settings;

        public PermanentTempMonitor()
        {
            settings = Program.container.GetInstance<ISettings>();

            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Elapsed += Elapsed;

            if (settings.TempMonEnabled) Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = new List<HardwareInfo>();

            foreach (ITemperatureProvider temperatureProvider in temperatureProviders)
            {
                result.AddRange(temperatureProvider.GetTemperature());
            }

            OnTemperatureChanged(result);
        }
    }
}
