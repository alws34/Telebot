using System.Text;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class MonitorsStatus : IStatus
    {
        public string Execute()
        {
            var result = new StringBuilder();

            foreach (ITempMon tempMon in Program.tempMons)
            {
                string name = tempMon.GetType().Name.Replace("TemperatureMonitor", "");
                string active = BoolToStr(tempMon.IsActive);
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
