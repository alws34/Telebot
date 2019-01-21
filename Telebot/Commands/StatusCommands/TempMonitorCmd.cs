using Telebot.Contracts;
using Telebot.Controllers;

namespace Telebot.Commands.StatusCommands
{
    public class TempMonitorCmd : IStatusCommand
    {
        private readonly ITemperatureMonitor tempMonitor;

        public TempMonitorCmd()
        {
            tempMonitor = Program.container.GetInstance<ITemperatureMonitor>();
        }

        public string Execute()
        {
            return $"*Monitor (°C)*: {tempMonitor.IsActive}";
        }
    }
}
