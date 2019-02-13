using System;
using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.Monitors
{
    public interface ITemperatureMonitor
    {
        event EventHandler<IEnumerable<HardwareInfo>> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}
