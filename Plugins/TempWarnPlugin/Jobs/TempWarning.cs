using BotSdk.Jobs;
using BotSdk.Settings;
using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System.Collections.Generic;
using TempWarnPlugin.Models;
using TempWarnPlugin.Settings;
using static CPUID.Sdk.CpuIdSdk64;

namespace TempWarnPlugin.Jobs
{
    public class TempWarning : IJob<TempArgs>, IProfile
    {
        private readonly float cpuLimit;
        private readonly float gpuLimit;

        private readonly Dictionary<uint, float> limits;
        private readonly IEnumerable<IDevice> devices;
        private readonly TempSettings settings;

        public TempWarning(IEnumerable<IDevice> devices)
        {
            this.devices = devices;

            settings = new TempSettings();
            settings.AddProfile(this);

            cpuLimit = settings.GetCpuLimit();
            gpuLimit = settings.GetGpuLimit();

            limits = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, cpuLimit },
                { CLASS_DEVICE_DISPLAY_ADAPTER, gpuLimit }
            };

            Active = settings.GetMonitoringState();

            if (Active)
            {
                StartJob();
            }
        }

        private void Elapsed()
        {
            foreach (IDevice device in devices)
            {
                Sensor sensor = device.GetSensor(SENSOR_CLASS_TEMPERATURE);

                bool success = limits.TryGetValue(device.DeviceClass, out float limit);

                if (success && sensor.Value >= limit)
                {
                    var args = new TempArgs
                    {
                        DeviceName = device.DeviceName,
                        Temperature = sensor.Value
                    };

                    RaiseUpdate(args);
                }
            }
        }

        public override void Start()
        {
            if (Active)
            {
                RaiseFeedback("Temperature is already being monitored.");
                return;
            }

            StartJob();
            Active = true;
        }

        public override void Stop()
        {
            if (!Active)
            {
                RaiseFeedback("Temperature is not being monitored.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);
            Active = false;
        }

        public void SaveChanges()
        {
            settings.SaveMonitoringState(Active);
            settings.SaveCpuLimit(cpuLimit);
            settings.SaveGpuLimit(gpuLimit);
        }

        private void StartJob()
        {
            JobManager.AddJob(
                Elapsed,
                s => s.WithName(GetType().Name).ToRunNow().AndEvery(7).Seconds()
            );
        }
    }
}
