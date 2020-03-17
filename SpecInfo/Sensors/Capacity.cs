using SpecInfo.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecInfo.Sensors
{
    public class Capacity : ISensor
    {
        public Capacity()
        {
            Name = "Capacities:";
            Class = SENSOR_CLASS_CAPACITY;
        }
    }
}
