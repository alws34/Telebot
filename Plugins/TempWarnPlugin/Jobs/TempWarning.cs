using Contracts;
using CPUID.Base;
using CPUID.Models;
using Enums;
using FluentScheduler;
using System.Collections.Generic;
using TempWarnPlugin.Models;
using static CPUID.Sdk.CpuIdSdk64;

namespace TempWarnPlugin.Jobs
{
    public class TempWarning : IJob<TempArgs>, IProfile
    {
        private float CPULimit;
        private float GPULimit;

        private readonly Dictionary<uint, float> limits;

        private readonly IEnumerable<IDevice> devices;

        public TempWarning(IEnumerable<IDevice> devices)
        {
            JobType = JobType.Fixed;

            this.devices = devices;

            GlobalSettings.Instance.Main.AddProfile(this);

            CPULimit = GlobalSettings.Instance.Temperature.GetCPULimit();
            GPULimit = GlobalSettings.Instance.Temperature.GetGPULimit();

            limits = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPULimit },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPULimit }
            };

            Active = GlobalSettings.Instance.Temperature.GetMonitoringState();

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
            };
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
            GlobalSettings.Instance.Temperature.SaveMonitoringState(Active);
            GlobalSettings.Instance.Temperature.SaveCPULimit(CPULimit);
            GlobalSettings.Instance.Temperature.SaveGPULimit(GPULimit);
        }

        private void StartJob()
        {
            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(7).Seconds()
            );
        }
    }
}
