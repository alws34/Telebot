using Plugins.NSSpec.Sensors.Contracts;
using static CPUID.Sdk.CpuIdSdk64;

namespace Plugins.NSSpec.Sensors
{
    public class ClockSpeed : ISensor
    {
        public ClockSpeed()
        {
            Name = "Clocks:";
            Class = SENSOR_CLASS_CLOCK_SPEED;
        }
    }
}
