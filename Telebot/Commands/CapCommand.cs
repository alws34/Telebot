using System;
using System.Drawing;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapCommand : ICommand
    {
        private readonly ScreenImpl screen;

        public CapCommand()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";

            screen = new ScreenImpl();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var screens = screen.CaptureDesktop();

            foreach (Bitmap scrn in screens)
            {
                var result = new Response
                {
                    ResultType = ResultType.Photo,
                    Raw = scrn.ToStream()
                };

                await resp(result);
            }
        }
    }
}
