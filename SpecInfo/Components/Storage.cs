using CPUID.Base;
using CPUID.Devices;
using System.Collections.Generic;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecInfo.Components
{
    public class Storage : IComponent
    {
        private readonly IEnumerable<IDevice> items;

        public Storage()
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_DRIVE);
        }

        public override string ToString()
        {
            foreach (HDDDevice device in items)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
