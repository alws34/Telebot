using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Contracts
{
    public interface IHardwareInfo
    {
        string DeviceName { get; set; }
        uint DeviceClass { get; set; }
        float Value { get; set; }
    }
}
