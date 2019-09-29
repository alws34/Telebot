using CPUID.Base;
using System.Text;

namespace CPUID.Devices
{
    public class GPUDevice : DeviceBase
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
            var strBuilder = new StringBuilder();

            var tempSensor = GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

            var gpuBrand = DeviceName.Split(' ')[0];
            float gpuTemp = tempSensor[0].Value;

            strBuilder.AppendLine($"*GPU {gpuBrand}*: {gpuTemp}°C");

            return strBuilder.ToString();
        }
    }
}
