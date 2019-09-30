using System;

namespace Telebot.Temperature
{
    public class TemperatureChangedArgs : EventArgs
    {
        public string DeviceName { get; set; }
        public float Temperature { get; set; }
    }
}
