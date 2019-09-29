using CPUID.Base;
using CPUID.Models;
using System;
using System.Text;

namespace CPUID.Devices
{
    public class HDDDevice : DeviceBase
    {
        public HDDDevice()
        {

        }

        public HDDDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            var sensors = GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);

            foreach (Sensor sensor in sensors)
            {
                string hddName = sensor.Name.ToUpper().Replace("SPACE", "Storage used");
                double hhdUtil = Math.Round(sensor.Value, 0);
                strBuilder.AppendLine($"*{hddName}*: {hhdUtil}%");
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}
