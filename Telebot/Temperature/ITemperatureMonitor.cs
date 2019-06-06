using System;

namespace Telebot.Temperature
{
    public interface ITemperatureMonitor
    {
        event EventHandler<TemperatureChangedArgs> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}