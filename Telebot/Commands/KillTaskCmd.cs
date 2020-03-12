using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class KillTaskCmd : ICommand
    {
        public KillTaskCmd()
        {
            Pattern = "/killtask (\\d+)";
            Description = "Kill a task with the specified pid.";
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            int pid = Convert.ToInt32(info.Groups[1].Value);

            Process target;

            var result = new Response
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
