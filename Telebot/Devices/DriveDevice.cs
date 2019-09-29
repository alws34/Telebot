using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telebot.Models;

namespace Telebot.Devices
{
    public class DriveDevice : DeviceBase
    {
        public DriveDevice()
        {

        }

        public DriveDevice(string DeviceName, int DeviceIndex, uint DeviceClass, int SensorCount)
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

            for (int i = 0; i < SensorsCount; i++)
            {
                var driveSensor = GetUtilizationSensors().ElementAt(i);
                string name = driveSensor.Name.ToUpper().Replace("SPACE", "Storage used");
                double drive_util = Math.Round(driveSensor.Value, 0);
                strBuilder.AppendLine($"*{name}*: {drive_util}%");
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}
