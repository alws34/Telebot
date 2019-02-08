using System;
using Telebot.Models;

namespace Telebot.Monitors
{
    public interface IScheduledTemperatureMonitor
    {
        event EventHandler<IHardwareInfo> TemperatureChanged;
        bool IsActive { get; }
        void Start(int durationInSec, int intervalInSec);
        void Stop();
    }
}
