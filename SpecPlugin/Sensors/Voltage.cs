using SpecPlugin.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecPlugin.Sensors
{
    public class Voltage : ISensor
    {
        public Voltage()
        {
            Name = "Voltages:";
            Class = SENSOR_CLASS_VOLTAGE;
        }
    }
}
