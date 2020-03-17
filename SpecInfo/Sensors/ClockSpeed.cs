using SpecInfo.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace SpecInfo.Sensors
{
    public class ClockSpeed : ISensor
    {
        public ClockSpeed()
        {
            Name = "Clocks";
            Class = SENSOR_CLASS_CLOCK_SPEED;
        }
    }
}
