using Telebot.Models;

namespace Telebot.HwProviders
{
    public class CPUProvider : ProviderBase, IDeviceProvider
    {
        public string DeviceName { get; set; }
        public int DeviceIndex { get; set; }
        public uint DeviceClass { get; set; }

        public CPUProvider(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public float GetTemperature()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_CLASS_TEMPERATURE, this.DeviceIndex);
        }

        public float GetUtilization()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_CLASS_UTILIZATION, this.DeviceIndex);
        }
    }
}
