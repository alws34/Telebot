using CPUID.Base;
using CPUID.Models;
using static CPUID.Sdk.CpuIdSdk64;

namespace CPUID.Devices
{
    public class RAMDevice : IMainboard
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
            Sensor sensor = GetSensor(SENSOR_CLASS_UTILIZATION);

            return $"*RAM Used.*: {sensor.Value}%";
        }
    }
}
