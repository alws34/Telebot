using CPUID.Models;
using System.Collections.Generic;

namespace CPUID.Contracts
{
    public interface IDevice
    {
        string DeviceName { get; }
        int DeviceIndex { get; }
        uint DeviceClass { get; }
        Sensor GetSensor(int sensorClass);
        List<Sensor> GetSensors(int sensorClass);
        string ToString();
    }
}
