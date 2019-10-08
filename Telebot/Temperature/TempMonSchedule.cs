using CPUID.Contracts;
using CPUID.Models;
using FluentScheduler;
using System;
using Telebot.Contracts;
using static CPUID.CPUIDSDK;

namespace Telebot.Temperature
{
    public class TempMonSchedule : TempMonBase, IScheduledJob
    {
        private DateTime timeStop;

        public TempMonSchedule(IDevice[] devices)
        {
            JobType = Common.JobType.Scheduled;

            this.devices.AddRange(devices);
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
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

            // Notify the observer(s) that we're done.
            RaiseTemperatureChanged(null);
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(interval.Seconds).Seconds()
            );

            IsActive = true;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }
    }
}
