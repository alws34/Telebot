using System;

namespace Telebot.Temperature
{
    public class TempChangedArgs : EventArgs
    {
        public string DeviceName { get; set; }
        public float Temperature { get; set; }
    }
}
