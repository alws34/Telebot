using SpecInfo.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecInfo.Sensors
{
    public class Power : ISensor
    {
        public Power()
        {
            Name = "Powers:";
            Class = SENSOR_CLASS_POWER;
        }
    }
}
