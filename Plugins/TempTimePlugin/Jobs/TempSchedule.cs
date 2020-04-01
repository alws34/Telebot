using Contracts.Factories;
using Contracts.Jobs;
using CPUID;
using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System;
using System.Collections.Generic;
using TempTimePlugin.Models;
using static CPUID.Sdk.CpuIdSdk64;

namespace TempTimePlugin.Jobs
{
    public class TempSchedule : IJob<TempArgs>, IScheduled
    {
        private DateTime timeStop;
        private readonly IEnumerable<IDevice> devices;

        public TempSchedule(IFactory<IDevice> deviceFactory)
        {
            devices = deviceFactory.FindAll(x =>
                (x.DeviceClass == CLASS_DEVICE_PROCESSOR) ||
                (x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER)
            );
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
                RaiseFeedback("temperature monitor finished.");
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

        public void Start(int durationSec, int intervalSec)
        {
            if (Active)
            {
                RaiseFeedback("Temperature monitor has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(durationSec);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(intervalSec).Seconds()
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
