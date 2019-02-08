using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public interface ITemperatureProvider
    {
        IEnumerable<HardwareInfo> GetTemperature();
    }
}
