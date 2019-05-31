using System;
using System.Collections.Generic;
using Telebot.HwProviders;

namespace Telebot.Temperature
{
    public interface ITemperatureMonitor
    {
        event EventHandler<IEnumerable<IDeviceProvider>> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}
