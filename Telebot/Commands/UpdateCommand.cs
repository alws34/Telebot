using AutoUpdaterDotNET;
using System;
using System.IO;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public class UpdateCommand : ICommand
    {
        public UpdateCommand()
        {
            Pattern = "/update (chk|dl)";
            Description = "Check or download an update.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string state = req.Groups[1].Value;

            if (state.Equals("chk"))
            {
                AutoUpdater.Start();
            }
            else if (state.Equals("dl"))
            {
                AutoUpdater.DownloadUpdate();

                var result = new Response
                {
                    ResultType = Common.ResultType.Text,
                    Text = "Updating Telebot..."
                };

                await resp(result);
            }
        }
    }
}
