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

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            int pid = Convert.ToInt32(info.Groups[1].Value);

            Process target;

            var result = new CommandResult
            {
                ResultType = ResultType.Text
            };

            try
            {
                target = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                result.Text = e.Message;
                await cbResult(result);
                return;
            }

            try
            {
                target.Kill();
                result.Text = $"Successfully killed {target.ProcessName}.";
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }

            await cbResult(result);
        }
    }
}
