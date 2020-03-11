using CPUID.Base;
using CPUID.Models;

namespace CPUID.Devices
{
    public class GPUDevice : IDevice
    {
        public GPUDevice()
        {

        }

        public GPUDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            Sensor sensor = GetSensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

            string name = DeviceName.Split(' ')[0];
            float value = sensor.Value;

            return $"*GPU {name}*: {value}°C";
        }
    }
}
