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

            string text = "";

            try
            {
                target = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                var result = new Response(e.Message);
                await resp(result);
                return;
            }

            try
            {
                target.Kill();
                text = $"{target.ProcessName} killed.";
            }
            catch (Exception e)
            {
                text = e.Message;
            }

            var result1 = new Response(text);
            await resp(result1);
        }
    }
}
