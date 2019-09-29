using CPUID.Contracts;
using System;
using System.Timers;

namespace Telebot.Temperature
{
    public class TemperatureMonitorDurated : TemperatureMonitorBase
    {
        private DateTime dtStop;

        public TemperatureMonitorDurated(params IDevice[][] devicesArr)
        {
            timer.Elapsed += Elapsed;

            foreach (IDevice[] devices in devicesArr)
            {
                this.devices.AddRange(devices);
            }
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= dtStop)
            {
                timer.Stop();
                return;
            }

            var parameter = new TemperatureChangedArgs
            {
                Devices = this.devices
            };

            RaiseTemperatureChanged(parameter);
        }

        public override void Start(TimeSpan duration, TimeSpan interval)
        {
            dtStop = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            Start();
        }
    }
}
