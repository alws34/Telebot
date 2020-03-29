using CPUID.Base;
using CPUID.Devices;
using SpecPlugin.Sensors.Contracts;
using System.Collections.Generic;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Components
{
    public class Storage : IComponent
    {
        private readonly IEnumerable<IDevice> items;
        private readonly IEnumerable<ISensor> sensors;

        public Storage(IEnumerable<ISensor> sensors)
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_DRIVE);
            this.sensors = sensors;
        }

        public override string ToString()
        {
            foreach (HDDDevice device in items)
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
