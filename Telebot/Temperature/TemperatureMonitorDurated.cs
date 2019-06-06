using System;
using System.Timers;
using Telebot.DeviceProviders;

namespace Telebot.Temperature
{
    public class TemperatureMonitorDurated : TemperatureMonitorBase
    {
        private DateTime dtStop;

        public TemperatureMonitorDurated(params IDeviceProvider[][] deviceProviders)
        {
            timer.Elapsed += Elapsed;

            foreach (IDeviceProvider[] devices in deviceProviders)
            {
                this.deviceProviders.AddRange(devices);
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
                Devices = this.deviceProviders
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
