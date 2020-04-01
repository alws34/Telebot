using CPUID.Base;
using CPUID.Models;
using static CPUID.Sdk.CpuIdSdk64;

namespace CPUID.Devices
{
    public class GPUDevice : IDisplay
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
            Sensor sensor = GetSensor(SENSOR_CLASS_TEMPERATURE);

            string name = DeviceName.Split(' ')[0];
            float value = sensor.Value;

            return $"*GPU {name}*: {value}°C";
        }
    }
}
