using Plugins.NSSpec.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace Plugins.NSSpec.Sensors
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
