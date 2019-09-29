﻿using System;
using System.Collections.Generic;
using Telebot.Devices;

namespace Telebot.Temperature
{
    public class TemperatureChangedArgs : EventArgs
    {
        public IEnumerable<IDevice> Devices;
    }
}
