using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Contracts
{
    public interface IUtilizationMonitor
    {
        void Start();
        void Stop();
        bool IsActive { get; }

        event EventHandler<IHardwareInfo> UtilizationChanged;
    }
}
