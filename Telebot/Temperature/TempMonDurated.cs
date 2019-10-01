using CPUID.Contracts;
using CPUID.Models;
using System;
using System.Timers;

using static CPUID.CPUIDSDK;

namespace Telebot.Temperature
{
    public class TempMonDurated : TempMonBase
    {
        private DateTime dtStop;

        public TempMonDurated(params IDevice[][] devicesArr)
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
                Sensor sensor = device.GetSensor(SENSOR_CLASS_TEMPERATURE);

                var args = new TempChangedArgs
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
