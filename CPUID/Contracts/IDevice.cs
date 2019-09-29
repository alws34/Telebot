using CPUID.Models;
using System.Collections.Generic;

namespace CPUID.Contracts
{
    public interface IDevice
    {
        string DeviceName { get; }
        int DeviceIndex { get; }
        uint DeviceClass { get; }
        List<Sensor> GetSensors(int SensorClass);
        string ToString();
    }
}
