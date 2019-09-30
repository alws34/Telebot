using CPUID.Contracts;
using CPUID.Models;
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

            foreach (IDevice device in devices)
            {
                Sensor sensor = device.GetSensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

                var args = new TemperatureChangedArgs
                {
                    DeviceName = device.DeviceName,
                    Temperature = sensor.Value
                };

                RaiseTemperatureChanged(args);
            }

            // Notify the observer that we're done.
            RaiseTemperatureChanged(null);
        }

        public override void Start(TimeSpan duration, TimeSpan interval)
        {
            dtStop = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            Start();
            Elapsed(this, null);
        }
    }
}
