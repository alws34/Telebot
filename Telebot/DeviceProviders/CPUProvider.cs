namespace Telebot.DeviceProviders
{
    public class CPUProvider : ProviderBase, IDeviceProvider
    {
        public string DeviceName { get; private set; }
        public int DeviceIndex { get; private set; }
        public uint DeviceClass { get; private set; }

        public CPUProvider()
        {

        }

        public CPUProvider(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public float GetTemperature()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_TEMPERATURE_CPU_DTS, this.DeviceIndex);
        }

        public float GetUtilization()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_UTILIZATION_CPU, this.DeviceIndex);
        }
    }
}
