using System;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.Monitors.Factories;

namespace Telebot.Commands
{
    public class TempMonOnCmd : CommandBase
    {
        private readonly ISettings settings;
        private readonly ITemperatureMonitor temperatureMonitor;

        public TempMonOnCmd()
        {
            Pattern = "/tempmon on";
            Description = "Turn on temperature monitoring.";
            settings = Program.container.GetInstance<ISettings>();
            temperatureMonitor = TempMonitorFactory.Instance.GetTemperatureMonitor<PermanentTempMonitor>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Temperature monitor is turned on.",
                SendType = SendType.Text
            };

            callback(result);

            temperatureMonitor.Start();
            settings.TempMonEnabled = true;
        }
    }
}
