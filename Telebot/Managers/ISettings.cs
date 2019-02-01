using System.Collections.Generic;
using System.Drawing;

namespace Telebot.Managers
{
    public interface ISettings
    {
        long ChatId { get; set; }
        float CPUTemperature { get; set; }
        float GPUTemperature { get; set; }
        bool MonitorEnabled { get; set; }
        Rectangle Form1Bounds { get; set; }
        List<int> ListView1ColumnsWidth { get; set; }
        string TelegramToken { get; }
        List<int> TelegramWhiteList { get; }
    }
}
