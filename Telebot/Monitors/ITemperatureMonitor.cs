using System;
using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.Monitors
{
    public interface ITemperatureMonitor
    {
        bool IsActive { get; }
        void Start(Action<IEnumerable<HardwareInfo>> callback);
        void Start(TimeSpan duration, TimeSpan interval, Action<IEnumerable<HardwareInfo>> callback);
        void Stop();
    }
}
