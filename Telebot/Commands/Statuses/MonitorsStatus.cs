namespace Telebot.Commands.Status
{
    public class MonitorsStatus : IStatus
    {
        public string Execute()
        {
            return $"*Monitor (°C)*: {Program.temperatureMonitorWarning.IsActive}";
        }
    }
}
