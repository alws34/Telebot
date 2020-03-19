using System;
using System.Drawing;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapCommand : ICommand
    {
        public CapCommand()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            var api = new DeskBmpApi();

            api.Invoke(async (screens) =>
            {
                foreach (Bitmap screen in screens)
                {
                    var result = new Response
                    {
                        ResultType = ResultType.Photo,
                        Raw = screen.ToStream()
                    };

                    await resp(result);
                }
            });
        }
    }
}
