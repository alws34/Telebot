using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System;
using System.Collections.Generic;
using Telebot.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace Telebot.Temperature
{
    public class TempSchedule : IJob<TempArgs>, IScheduled
    {
        private DateTime timeStop;
        private readonly IEnumerable<IDevice> devices;

        public TempSchedule(IEnumerable<IDevice> devices)
        {
            JobType = Common.JobType.Scheduled;

            this.devices = devices;
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
            if (Active)
            {
                RaiseFeedback("Temperature monitor has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            int seconds = Convert.ToInt32(interval.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(seconds).Seconds()
            );

            Active = true;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            if (!Active)
            {
                RaiseFeedback("temperature monitor is not scheduled.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);

            Active = false;
        }
    }
}
