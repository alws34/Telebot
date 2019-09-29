using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.DeviceProviders
{
    public class RAMProvider : ProviderBase
    {
        public RAMProvider()
        {

        }

        public RAMProvider(string DeviceName, int DeviceIndex, uint DeviceClass, int SensorCount)
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

            float ram_util = GetUtilizationSensors().ElementAt(0).Value;
            strBuilder.AppendLine($"*RAM Used.*: {ram_util}%");

            return strBuilder.ToString().TrimEnd();
        }
    }
}
