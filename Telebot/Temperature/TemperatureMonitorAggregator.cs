using CPUID.Contracts;
using CPUID.Models;
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
            foreach (IDevice device in e.Devices)
            {
                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                Sensor sensor = sensors[0];

                string text = $"*[WARNING] {device.DeviceName}*: {sensor.Value}°C\nFrom *Telebot*";

                await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text, ParseMode.Markdown);
            }
        }

        private async void DuratedTemperatureChanged(object sender, TemperatureChangedArgs e)
        {
            var text = new StringBuilder();

            foreach (IDevice device in e.Devices)
            {
                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                Sensor sensor = sensors[0];

                text.AppendLine($"*{device.DeviceName}*: {sensor.Value}°C");
            }

            text.AppendLine("\nFrom *Telebot*");

            await telebotClient.SendTextMessageAsync(telebotClient.AdminID, text.ToString(), ParseMode.Markdown);
        }
    }
}
