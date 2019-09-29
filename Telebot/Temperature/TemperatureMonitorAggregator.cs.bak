using System.Linq;
using System.Text;
using Telebot.Clients;
using Telebot.DeviceProviders;
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
            foreach (IDeviceProvider device in e.Devices)
            {
                string deviceName = device.DeviceName;
                var sensors = device.GetTemperatureSensors();
                SensorInfo package = sensors.ElementAt(0);
                float temperature = package.Value;

                string text = $"*[WARNING] {deviceName}*: {temperature}°C\nFrom *Telebot*";

                await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text, ParseMode.Markdown);
            }
        }

        private async void DuratedTemperatureChanged(object sender, TemperatureChangedArgs e)
        {
            var text = new StringBuilder();

            foreach (IDeviceProvider device in e.Devices)
            {
                string deviceName = device.DeviceName;
                var sensors = device.GetTemperatureSensors();
                SensorInfo package = sensors.ElementAt(0);
                float temperature = package.Value;

                text.AppendLine($"*{deviceName}*: {temperature}°C");
            }

            text.AppendLine("\nFrom *Telebot*");

            await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text.ToString(), ParseMode.Markdown);
        }
    }
}
