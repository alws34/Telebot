using System.Collections.Generic;
using System.Linq;

namespace Telebot.Monitors.Factories
{
    public sealed class TempMonitorFactory
    {
        private readonly IEnumerable<ITemperatureMonitor> temperatureMonitors;

        public static TempMonitorFactory Instance { get; } = new TempMonitorFactory();

        TempMonitorFactory()
        {
            temperatureMonitors = Program.container.GetAllInstances<ITemperatureMonitor>();
        }

        public ITemperatureMonitor GetTemperatureMonitor<T>()
        {
            return temperatureMonitors.Single(x => x is T);
        }

        public IEnumerable<ITemperatureMonitor> GetAllTemperatureMonitors()
        {
            return temperatureMonitors;
        }
    }
}
