using SpecPlugin.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Sensors
{
    public class Utilization : ISensor
    {
        public Utilization()
        {
            Name = "Utilization:";
            Class = SENSOR_CLASS_UTILIZATION;
        }
    }
}
