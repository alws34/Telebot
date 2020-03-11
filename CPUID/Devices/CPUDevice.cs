using CPUID.Base;
using System;

namespace CPUID.Devices
{
    public class CPUDevice : IDevice
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
            var utilSensor = GetSensor(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
            var tempSensor = GetSensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

            string result = "";
            result += $"*CPU Usage*: {Math.Round(utilSensor.Value, 0)}%\n";
            result += $"*CPU Temp.*: {tempSensor.Value}°C";

            return result;
        }
    }
}
