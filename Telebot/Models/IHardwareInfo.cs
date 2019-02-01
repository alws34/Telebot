namespace Telebot.Models
{
    public interface IHardwareInfo
    {
        string DeviceName { get; set; }
        uint DeviceClass { get; set; }
        float Value { get; set; }
    }
}
