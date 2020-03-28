using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCommand : ICommand
    {
        public CapAppCommand()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified app (by pid).";
            OSVersion = new Version(5, 0);
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var api = new WindowApi(hWnd);

            api.Invoke(async (wnd) =>
            {
                var result = new Response(wnd.ToMemStream());

                await resp(result);
            });
        }
    }
}
