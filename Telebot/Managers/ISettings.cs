using System.Collections.Generic;
using System.Drawing;

namespace Telebot.Managers
{
    public interface ISettings
    {
        string TelegramToken { get; }
        List<long> TelegramWhitelist { get; }
        Rectangle Form1Bounds { get; set; }
        List<int> ListView1ColumnsWidth { get; set; }
        float CPUTemperature { get; set; }
        float GPUTemperature { get; set; }
        bool TempMonEnabled { get; set; }
    }
}
