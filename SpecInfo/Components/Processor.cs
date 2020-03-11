using CPUID.Devices;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Processor : IComponent
    {
        public override string ToString()
        {
            foreach (CPUDevice device in DeviceFactory.CPUDevices)
            {
                stringResult.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(SENSOR_CLASS_VOLTAGE);
                AppendSensors("Voltages:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_POWER);
                AppendSensors("Powers:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);

                sensors = device.GetSensors(SENSOR_CLASS_CLOCK_SPEED);
                AppendSensors("Clocks:", sensors);
            }

            return stringResult.ToString();
        }
    }
}
