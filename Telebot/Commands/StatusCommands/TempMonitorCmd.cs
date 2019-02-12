using Telebot.Monitors;
using Telebot.Monitors.Factories;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class TempMonitorCmd : IStatusCommand
    {
        private readonly ITemperatureMonitor temperatureMonitor;

        public TempMonitorCmd()
        {
            temperatureMonitor = TempMonitorFactory.Instance.GetTemperatureMonitor<PermanentTempMonitor>();
        }

        public string Execute()
        {
            return $"*Monitor (°C)*: {temperatureMonitor.IsActive}";
        }
    }
}
