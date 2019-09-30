using System.Text;
using Telebot.Clients;
using Telegram.Bot.Types.Enums;

namespace Telebot.Temperature
{
    public class TemperatureMonitorAggregator
    {
        private readonly ITelebotClient telebotClient;

        public TemperatureMonitorAggregator(ITelebotClient telebotClient, params ITemperatureMonitor[] temperatureMonitors)
        {
            this.telebotClient = telebotClient;

            foreach (ITemperatureMonitor temperatureMonitor in temperatureMonitors)
            {
                if (temperatureMonitor is TemperatureMonitorWarning)
                {
                    temperatureMonitor.TemperatureChanged += WarningTemperatureChanged;
                }
                else if (temperatureMonitor is TemperatureMonitorDurated)
                {
                    temperatureMonitor.TemperatureChanged += DuratedTemperatureChanged;
                }
            }
        }

        private async void WarningTemperatureChanged(object sender, TemperatureChangedArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text, ParseMode.Markdown);
        }

        private StringBuilder text = new StringBuilder();

        private async void DuratedTemperatureChanged(object sender, TemperatureChangedArgs e)
        {
            if (e != null)
            {
                text.AppendLine($"*{e.DeviceName}*: {e.Temperature}°C");
            }
            else
            {
                text.AppendLine("\nFrom *Telebot*");

                await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text.ToString(), ParseMode.Markdown);

                text.Clear();
            }
        }
    }
}
