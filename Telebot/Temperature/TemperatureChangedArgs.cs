using CPUID.Contracts;
using System;
using System.Collections.Generic;

namespace Telebot.Temperature
{
    public class TemperatureChangedArgs : EventArgs
    {
        public IEnumerable<IDevice> Devices;
    }
}
