using SpecPlugin.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Sensors
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
