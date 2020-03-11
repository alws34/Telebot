using CPUID.Devices;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Mainboard : IComponent
    {
        public override string ToString()
        {
            foreach (RAMDevice device in DeviceFactory.RAMDevices)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
