using SpecInfo.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecInfo.Sensors
{
    public class Temperature : ISensor
    {
        public Temperature()
        {
            Name = "Temperatures:";
            Class = SENSOR_CLASS_TEMPERATURE;
        }
    }
}
