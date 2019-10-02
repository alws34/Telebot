using System;

namespace Telebot.Temperature
{
    public interface ITempMon
    {
        event EventHandler<TempChangedArgs> TemperatureChanged;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}