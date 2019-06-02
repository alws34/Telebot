namespace Telebot.DeviceProviders
{
    public class GPUProvider : ProviderBase, IDeviceProvider
    {
        public string DeviceName { get; private set; }
        public int DeviceIndex { get; private set; }
        public uint DeviceClass { get; private set; }

        public GPUProvider()
        {

        }

        public GPUProvider(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public float GetTemperature()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_TEMPERATURE_GPU, this.DeviceIndex);
        }

        public float GetUtilization()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_UTILIZATION_GPU, this.DeviceIndex);
        }
    }
}
