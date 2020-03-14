using CPUID.Base;
using CPUID.Devices;
using System.Collections.Generic;
using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Mainboard : IComponent
    {
        private readonly IEnumerable<IDevice> items;

        public Mainboard()
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_MAINBOARD);
        }

        public override string ToString()
        {
            foreach (RAMDevice device in items)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
