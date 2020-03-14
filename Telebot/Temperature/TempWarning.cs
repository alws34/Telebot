using CPUID.Base;
using CPUID.Models;
using FluentScheduler;
using System.Collections.Generic;
using Telebot.Settings;
using static CPUID.CPUIDSDK;

namespace Telebot.Temperature
{
    public class TempWarning : BaseTemp, IProfile
    {
        private float CPULimit;
        private float GPULimit;

        private readonly TempSettings settings;
        private readonly Dictionary<uint, float> limits;

        public TempWarning(IEnumerable<IDevice> devices, TempSettings settings)
        {
            JobType = Common.JobType.Fixed;

            this.settings = settings;

            CPULimit = settings.GetCPULimit();
            GPULimit = settings.GetGPULimit();

            limits = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPULimit },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPULimit }
            };

            Program.Settings.Handler.AddProfile(this);

            this.devices.AddRange(devices);

            IsActive = Program.Settings.Temperature.GetMonitoringState();

            if (IsActive)
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

                    RaiseTemperatureChanged(args);
                }
            };
        }

        public override void Start()
        {
            if (IsActive)
            {
                RaiseNotify("Temperature is already being monitored.");
                return;
            }

            StartJob();
            IsActive = true;
        }

        public override void Stop()
        {
            if (!IsActive)
            {
                RaiseNotify("Temperature is not being monitored.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);
            IsActive = false;
        }

        public void SaveChanges()
        {
            settings.SaveMonitoringState(IsActive);
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
