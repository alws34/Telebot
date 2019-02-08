using System;
using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.ScheduledOperations
{
    public interface IScheduledTemperatureMonitor
    {
        event EventHandler<IEnumerable<HardwareInfo>> TemperatureChanged;
        bool IsActive { get; }
        void Start(int durationInSec, int intervalInSec);
        void Stop();
    }
}
