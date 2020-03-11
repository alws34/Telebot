using CPUID.Devices;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Storage : IComponent
    {
        public override string ToString()
        {
            foreach (HDDDevice device in DeviceFactory.HDDDevices)
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
