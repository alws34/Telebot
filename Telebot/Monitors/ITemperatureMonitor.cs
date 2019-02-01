using System;
using Telebot.Models;

namespace Telebot.Monitors
{
    public interface ITemperatureMonitor
    {
        void Start();
        void Stop();
        bool IsActive { get; }

        event EventHandler<IHardwareInfo> TemperatureChanged;
    }
}
