using Plugins.NSSpec.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace Plugins.NSSpec.Sensors
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
