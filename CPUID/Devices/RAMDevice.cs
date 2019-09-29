using CPUID.Base;
using System.Text;

namespace CPUID.Devices
{
    public class RAMDevice : DeviceBase
    {
        public RAMDevice()
        {

        }

        public RAMDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            var utilSensor = GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);

            float ramUtil = utilSensor[0].Value;

            strBuilder.AppendLine($"*RAM Used.*: {ramUtil}%");

            return strBuilder.ToString().TrimEnd();
        }
    }
}
