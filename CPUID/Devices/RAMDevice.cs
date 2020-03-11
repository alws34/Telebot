using CPUID.Base;
using CPUID.Models;

namespace CPUID.Devices
{
    public class RAMDevice : IDevice
    {
        public RAMDevice()
        {

        }

        public RAMDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            Sensor sensor = GetSensor(CPUIDSDK.SENSOR_CLASS_UTILIZATION);

            return $"*RAM Used.*: {sensor.Value}%";
        }
    }
}
