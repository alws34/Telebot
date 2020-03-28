using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class KillCommand : ICommand
    {
        public KillCommand()
        {
            Pattern = "/kill (\\d+)";
            Description = "Kill a task with the specified pid.";
            OSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

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
                await resp(result);
                return;
            }

            try
            {
                target.Kill();
                result.Text = $"{target.ProcessName} killed.";
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }

            await resp(result);
        }
    }
}
