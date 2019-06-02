namespace Telebot.DeviceProviders
{
    public interface IDeviceProvider
    {
        string DeviceName { get; }
        int DeviceIndex { get; }
        uint DeviceClass { get; }
        float GetTemperature();
        float GetUtilization();
    }
}
