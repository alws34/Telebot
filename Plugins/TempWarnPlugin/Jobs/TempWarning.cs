﻿using Contracts;
using CPUID;
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
        private float CPULimit;
        private float GPULimit;

        private readonly Dictionary<uint, float> limits;
        private readonly IEnumerable<IDevice> devices;
        private readonly TempSettings settings;

        public TempWarning()
        {
            devices = CpuIdWrapper64.DeviceFactory.FindAll(x =>
               (x.DeviceClass == CLASS_DEVICE_PROCESSOR) ||
               (x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER)
            );

            settings = new TempSettings();
            settings.AddProfile(this);

            CPULimit = settings.GetCPULimit();
            GPULimit = settings.GetGPULimit();

            limits = new Dictionary<uint, float>
            {
                { CLASS_DEVICE_PROCESSOR, CPULimit },
                { CLASS_DEVICE_DISPLAY_ADAPTER, GPULimit }
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