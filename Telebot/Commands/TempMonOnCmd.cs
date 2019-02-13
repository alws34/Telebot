using System;
using System.Collections.Generic;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOnCmd : CommandBase
    {
        public TempMonOnCmd()
        {
            Pattern = "/tempmon on";
            Description = "Turn on temperature monitoring.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            void temperatureChanged(IEnumerable<HardwareInfo> devices)
            {
                foreach (HardwareInfo device in devices)
                {
                    string text = $"*[WARNING] {device.DeviceName}*: {device.Value}°C\nFrom *Telebot*";
                    var res = new CommandResult
                    {
                        Text = text,
                        SendType = SendType.Text
                    };

                    callback(res);
                }
            }

            var result = new CommandResult
            {
                Text = "Temperature monitor is turned on.",
                SendType = SendType.Text
            };

            callback(result);

            PermanentTempMonitor.Instance.Start(temperatureChanged);
            Program.appSettings.TempMonEnabled = true;
        }
    }
}
