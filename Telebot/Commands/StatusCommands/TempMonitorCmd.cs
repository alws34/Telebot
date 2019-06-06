using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class TempMonitorCmd : IStatusCommand
    {
        public string Execute()
        {
            return $"*Monitor (°C)*: {Program.temperatureMonitorWarning.IsActive}";
        }
    }
}
