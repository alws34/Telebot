using SpecPlugin.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Sensors
{
    public class Level : ISensor
    {
        public Level()
        {
            Name = "Levels:";
            Class = SENSOR_CLASS_LEVEL;
        }
    }
}
