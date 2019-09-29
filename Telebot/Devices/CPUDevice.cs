using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telebot.Models;

namespace Telebot.Devices
{
    public class CPUDevice : DeviceBase
    {
        public CPUDevice()
        {

        }

        public CPUDevice(string DeviceName, int DeviceIndex, uint DeviceClass, int SensorCount)
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

            float cpu_util = GetUtilizationSensors().ElementAt(0).Value;
            double cpu_util_round = Math.Round(cpu_util, 0);

            strBuilder.AppendLine($"*CPU Usage*: {cpu_util_round}%");

            float cpu_temp = GetTemperatureSensors().ElementAt(0).Value;

            strBuilder.AppendLine($"*CPU Temp.*: {cpu_temp}°C");

            return strBuilder.ToString().TrimEnd();
        }
    }
}
