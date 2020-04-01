using CPUID.Base;
using System;
using static CPUID.Sdk.CpuIdSdk64;

namespace CPUID.Devices
{
    public class CPUDevice : IProcessor
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
            var utilSensor = GetSensor(SENSOR_CLASS_UTILIZATION);
            var tempSensor = GetSensor(SENSOR_CLASS_TEMPERATURE);

            string result = "";
            result += $"*CPU Usage*: {Math.Round(utilSensor.Value, 0)}%\n";
            result += $"*CPU Temp.*: {tempSensor.Value}°C";

            return result;
        }
    }
}
