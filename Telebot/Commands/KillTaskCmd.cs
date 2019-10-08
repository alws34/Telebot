using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
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
                await callback(cmdResult);
                return;
            }

            try
            {
                target.Kill();
                cmdResult.Text = $"Successfully killed {target.ProcessName}.";
            }
            catch (Exception e)
            {
                cmdResult.Text = e.Message;
            }

            await callback(cmdResult);
        }
    }
}
