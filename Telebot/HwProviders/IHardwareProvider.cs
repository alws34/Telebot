using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public interface IHardwareProvider
    {
        IEnumerable<HardwareInfo> GetTemperature();

        IEnumerable<HardwareInfo> GetUtilization();
    }
}
