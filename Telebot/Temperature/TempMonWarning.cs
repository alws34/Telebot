using CPUID.Contracts;
using CPUID.Devices;
using CPUID.Models;
using FluentScheduler;
using System;
using System.Collections.Generic;
using static CPUID.CPUIDSDK;
using static Telebot.Settings.SettingsFactory;

namespace Telebot.Temperature
{
    public class TempMonWarning : TempMonBase, Settings.IProfile
    {
        private float CPUWarningLevel;
        private float GPUWarningLevel;

        private readonly Dictionary<uint, float> tempWarningLevels;

        public TempMonWarning(IDevice[] devices)
        {
            JobType = Common.JobType.Fixed;

            CPUWarningLevel = MonitorSettings.GetCPUWarningLevel();
            GPUWarningLevel = MonitorSettings.GetGPUWarningLevel();

            tempWarningLevels = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPUWarningLevel },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPUWarningLevel }
            };

            SettingsBase.AddProfile(this);

            this.devices.AddRange(devices);

            IsActive = MonitorSettings.GetTempMonitorStatus();

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
                    var args = new TempChangedArgs
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
            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(7).Seconds()
            );

            IsActive = true;
        }

        public override void Stop()
        {
            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public void SaveChanges()
        {
            MonitorSettings.SaveTempMonitorStatus(IsActive);
            MonitorSettings.SaveCPUWarningLevel(CPUWarningLevel);
            MonitorSettings.SaveGPUWarningLevel(GPUWarningLevel);
        }
    }
}
