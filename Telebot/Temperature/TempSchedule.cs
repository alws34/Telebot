using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System;
using System.Collections.Generic;
using Telebot.Contracts;
using static CPUID.CPUIDSDK;

namespace Telebot.Temperature
{
    public class TempSchedule : BaseTemp, IScheduled
    {
        private DateTime timeStop;

        public TempSchedule(IEnumerable<IDevice> devices)
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

                var args = new TempArgs
                {
                    DeviceName = device.DeviceName,
                    Temperature = sensor.Value
                };

                RaiseUpdate(args);
            }

            // Notify the observer(s) that we're done.
            RaiseUpdate(null);
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            if (IsActive)
            {
                RaiseNotify("Temperature monitor has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            int seconds = Convert.ToInt32(interval.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(seconds).Seconds()
            );

            IsActive = true;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            if (!IsActive)
            {
                RaiseNotify("temperature monitor is not scheduled.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }
    }
}
