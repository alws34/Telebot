using System.Collections.Generic;

namespace Telebot.Contracts
{
    public interface IUtilizationProvider
    {
        List<IHardwareInfo> GetUtilization();
    }
}
