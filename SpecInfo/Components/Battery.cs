using CPUID.Base;
using CPUID.Devices;
using System.Collections.Generic;
using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace SpecInfo.Components
{
    public class Battery : IComponent
    {
        private readonly IEnumerable<IDevice> items;

        public Battery()
        {
            items = DeviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_BATTERY);
        }

        public override string ToString()
        {
            foreach (BATDevice device in items)
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
