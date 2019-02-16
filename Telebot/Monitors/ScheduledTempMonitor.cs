using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.Monitors
{
    public class ScheduledTempMonitor : TemperatureMonitorBase
    {
        private DateTime dtStop;

        public static ITemperatureMonitor Instance { get; } = new ScheduledTempMonitor();

        ScheduledTempMonitor()
        {
            timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= dtStop)
            {
                timer.Stop();
                return;
            }

            var result = new List<HardwareInfo>();

            foreach (IHardwareProvider temperatureProvider in temperatureProviders)
            {
                result.AddRange(temperatureProvider.GetTemperature());
            }

            OnTemperatureChanged(result);
        }

        public override void Start(TimeSpan duration, TimeSpan interval)
        {
            dtStop = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            Start();
        }
    }
}
