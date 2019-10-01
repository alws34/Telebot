using System;

namespace Telebot.Temperature
{
    public interface ITempMon
    {
        event EventHandler<TempChangedArgs> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}