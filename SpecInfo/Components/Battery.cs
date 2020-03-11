using CPUID.Devices;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Battery : IComponent
    {
        public override string ToString()
        {
            foreach (BATDevice device in DeviceFactory.BATDevices)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_VOLTAGE);
                AppendSensors("Voltages:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_CAPACITY);
                AppendSensors("Capacities:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_LEVEL);
                AppendSensors("Levels:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
