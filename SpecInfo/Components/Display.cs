using CPUID.Base;
using CPUID.Devices;
using System.Collections.Generic;
using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Display : IComponent
    {
        private readonly IEnumerable<IDevice> items;

        public Display()
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER);
        }

        public override string ToString()
        {
            foreach (GPUDevice device in items)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
