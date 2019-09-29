using CPUID.Base;
using System;
using System.Text;

namespace CPUID.Devices
{
    public class CPUDevice : DeviceBase
    {
        public CPUDevice()
        {

        }

        public CPUDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            var utilSensors = GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
            var tempSensors = GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

            float cpuUtil = utilSensors[0].Value;
            float cpuTemp = tempSensors[0].Value;

            strBuilder.AppendLine($"*CPU Usage*: {Math.Round(cpuUtil, 0)}%");
            strBuilder.AppendLine($"*CPU Temp.*: {cpuTemp}°C");

            return strBuilder.ToString().TrimEnd();
        }
    }
}
