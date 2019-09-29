using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telebot.Models;

namespace Telebot.Devices
{
    public class GPUDevice : DeviceBase
    {
        public GPUDevice()
        {

        }

        public GPUDevice(string DeviceName, int DeviceIndex, uint DeviceClass, int SensorCount)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
            this.SensorsCount = SensorCount;
        }

        public override IEnumerable<SensorInfo> GetTemperatureSensors()
        {
            return base.GetSensorsInfo(CPUIDSDK.SENSOR_CLASS_TEMPERATURE, this.DeviceIndex);
        }

        public override IEnumerable<SensorInfo> GetUtilizationSensors()
        {
            return base.GetSensorsInfo(CPUIDSDK.SENSOR_CLASS_UTILIZATION, this.DeviceIndex);
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            var gpuBrand = DeviceName.Split(' ')[0];
            float gpu_temp = GetTemperatureSensors().ElementAt(0).Value;
            strBuilder.AppendLine($"*GPU {gpuBrand}*: {gpu_temp}°C");

            return strBuilder.ToString();
        }
    }
}
