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
        private float CPUWarningLevel;
        private float GPUWarningLevel;

        private readonly Dictionary<uint, float> tempWarningLevels;

        public TempWarning(IDevice[] devices)
        {
            JobType = Common.JobType.Fixed;

            CPUWarningLevel = Program.Settings.WarnMon.GetCPUWarningLevel();
            GPUWarningLevel = Program.Settings.WarnMon.GetGPUWarningLevel();

            tempWarningLevels = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPUWarningLevel },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPUWarningLevel }
            };

            Program.Settings.Handler.AddProfile(this);

            this.devices.AddRange(devices);

            IsActive = Program.Settings.WarnMon.GetTempMonitorStatus();

            if (IsActive)
            {
                Start();
            }
        }

        private void Elapsed()
        {
            foreach (IDevice device in devices)
            {
                Sensor sensor = device.GetSensor(SENSOR_CLASS_TEMPERATURE);

                bool success = tempWarningLevels.TryGetValue(device.DeviceClass, out float warningTemp);

                if (success && sensor.Value >= warningTemp)
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
                RaiseNotify("Temperature monitor is already monitoring.");
                return;
            }

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(7).Seconds()
            );

            IsActive = true;
        }

        public override void Stop()
        {
            if (!IsActive)
            {
                RaiseNotify("Temperature monitor is not monitoring.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public void SaveChanges()
        {
            Program.Settings.WarnMon.SaveTempMonitorStatus(IsActive);
            Program.Settings.WarnMon.SaveCPUWarningLevel(CPUWarningLevel);
            Program.Settings.WarnMon.SaveGPUWarningLevel(GPUWarningLevel);
        }
    }
}
