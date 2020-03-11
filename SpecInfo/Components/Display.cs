using CPUID.Devices;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Display : IComponent
    {
        public override string ToString()
        {
            foreach (GPUDevice device in DeviceFactory.GPUDevices)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
