using System.Collections.Generic;

namespace Telebot.DeviceProviders
{
    public interface IDeviceProvider
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
