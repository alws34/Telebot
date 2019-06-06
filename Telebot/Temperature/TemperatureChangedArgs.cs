using System;
using System.Collections.Generic;
using Telebot.DeviceProviders;

namespace Telebot.Temperature
{
    public class TemperatureChangedArgs : EventArgs
    {
        public IEnumerable<IDeviceProvider> Devices;
    }
}
