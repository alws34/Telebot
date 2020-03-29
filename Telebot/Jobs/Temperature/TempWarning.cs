using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System.Collections.Generic;
using Telebot.Jobs;
using Telebot.Settings;
using static CPUID.Sdk.CpuIdSdk64;

namespace Telebot.Temperature
{
    public class TempWarning : IJob<TempArgs>, IProfile
    {
        private float CPULimit;
        private float GPULimit;

        private readonly TempSettings settings;
        private readonly Dictionary<uint, float> limits;

        private readonly IEnumerable<IDevice> devices;

        public TempWarning(IEnumerable<IDevice> devices, TempSettings settings)
        {
            JobType = Common.JobType.Fixed;

            this.devices = devices;

            this.settings = settings;
            Program.AppSettings.Main.AddProfile(this);

            CPULimit = settings.GetCPULimit();
            GPULimit = settings.GetGPULimit();

            limits = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPULimit },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPULimit }
            };

            Active = Program.AppSettings.Temperature.GetMonitoringState();

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
            settings.SaveMonitoringState(Active);
            settings.SaveCPULimit(CPULimit);
            settings.SaveGPULimit(GPULimit);
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
