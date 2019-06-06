using System;
using System.Diagnostics;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class KillTaskCmd : CommandBase
    {
        public KillTaskCmd()
        {
            Pattern = "/killtask (\\d+)";
            Description = "Kill a task with the specified pid.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            var pid = Convert.ToInt32(parameters.Groups[1].Value);

            Process target;

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text
            };

            try
            {
                target = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                cmdResult.Text = e.Message;
                callback(cmdResult);
                return;
            }

            try
            {
                target.Kill();
                cmdResult.Text = $"Successfully killed {target.ProcessName}.";
            }
            catch
            {
                cmdResult.Text = $"Unsuccessfully killed {target.ProcessName}.";
            }

            callback(cmdResult);
        }
    }
}
