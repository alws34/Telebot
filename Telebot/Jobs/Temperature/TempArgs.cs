using System;

namespace Telebot.Temperature
{
    public class TempArgs : EventArgs
    {
        public string DeviceName { get; set; }
        public float Temperature { get; set; }
    }
}
