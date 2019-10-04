using System;

namespace Telebot.Temperature
{
    public enum TempMonType
    {
        Warning,
        Scheduled
    }

    public interface ITempMon
    {
        event EventHandler<TempChangedArgs> TemperatureChanged;
        TempMonType TempMonType { get; }
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}