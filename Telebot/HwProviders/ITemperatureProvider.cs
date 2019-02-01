using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public interface ITemperatureProvider
    {
        List<IHardwareInfo> GetTemperature();
    }
}
