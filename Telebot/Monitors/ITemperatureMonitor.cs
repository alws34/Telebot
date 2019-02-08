using System;
using Telebot.Models;

namespace Telebot.Monitors
{
    public interface ITemperatureMonitor
    {
        event EventHandler<HardwareInfo> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
