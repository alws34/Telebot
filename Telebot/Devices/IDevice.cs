using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.Devices
{
    public interface IDevice
    {
        string DeviceName { get; }
        int DeviceIndex { get; }
        uint DeviceClass { get; }
        int SensorsCount { get; }
        IEnumerable<SensorInfo> GetTemperatureSensors();
        IEnumerable<SensorInfo> GetUtilizationSensors();
        string ToString();
    }
}
