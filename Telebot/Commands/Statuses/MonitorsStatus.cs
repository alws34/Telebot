using System.Text;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class MonitorsStatus : IStatus
    {
        public string Execute()
        {
            var result = new StringBuilder();

            foreach (ITemperatureMonitor temperatureMonitor in Program.temperatureMonitors)
            {
                string name = temperatureMonitor.GetType().Name.Replace("TemperatureMonitor", "");
                string active = BoolToStr(temperatureMonitor.IsActive);
                result.AppendLine($"*{name} Monitor (°C)*: {active}");
            }

            return result.ToString();
        }

        private string BoolToStr(bool active)
        {
            return active ? "Active" : "Inactive";
        }
    }
}
