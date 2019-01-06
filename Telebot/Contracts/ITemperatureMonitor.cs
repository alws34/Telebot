using System;

namespace Telebot.Contracts
{
    public interface ITemperatureMonitor
    {
        void Start();
        void Stop();
        bool IsActive { get; }

        event EventHandler<IHardwareInfo> TemperatureChanged;
    }
}
