﻿using CPUID.Base;
using CPUID.Devices;
using SpecPlugin.Sensors.Contracts;
using System.Collections.Generic;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Components
{
    public class Battery : IComponent
    {
        private readonly IEnumerable<IDevice> items;
        private readonly IEnumerable<ISensor> sensors;

        public Battery(IEnumerable<ISensor> sensors)
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_BATTERY);
            this.sensors = sensors;
        }

        public override string ToString()
        {
            foreach (BATDevice device in items)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                foreach (ISensor sensor in sensors)
                {
                    var result = device.GetSensors(sensor.Class);
                    AppendSensors(sensor.Name, result);
                }
            }

            return stringResult.ToString();
        }
    }
}