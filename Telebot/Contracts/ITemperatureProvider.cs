using System.Collections.Generic;

namespace Telebot.Contracts
{
    public interface ITemperatureProvider
    {
        List<IHardwareInfo> GetTemperature();
    }
}
