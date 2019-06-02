namespace Telebot.HwProviders
{
    public interface IDeviceProvider
    {
        string DeviceName { get; set; }
        int DeviceIndex { get; set; }
        uint DeviceClass { get; set; }
        float GetTemperature();
        float GetUtilization();
    }
}
