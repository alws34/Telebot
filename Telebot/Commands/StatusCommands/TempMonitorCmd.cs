using Telebot.Monitors;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class TempMonitorCmd : IStatusCommand
    {
        private readonly IScheduledTemperatureMonitor tempMonitor;

        public TempMonitorCmd()
        {
            tempMonitor = Program.container.GetInstance<IScheduledTemperatureMonitor>();
        }

        public string Execute()
        {
            return $"*Monitor (°C)*: {tempMonitor.IsActive}";
        }
    }
}
