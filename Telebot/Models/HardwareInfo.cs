namespace Telebot.Models
{
    public class HardwareInfo : IHardwareInfo
    {
        public string DeviceName { get; set; }
        public uint DeviceClass { get; set; }
        public float Value { get; set; }
    }
}
