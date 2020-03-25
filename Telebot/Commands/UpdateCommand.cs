using AutoUpdaterDotNET;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            switch(state)
            {
                case "chk":
                    AutoUpdater.Start();
                    break;
                case "dl":
                    var result = new Response
                    {
                        ResultType = Common.ResultType.Text,
                        Text = "Updating Telebot..."
                    };

                    await resp(result);

                    AutoUpdater.DownloadUpdate();

                    JobManager.AddJob(
                        () =>
                        {
                            Application.Exit();
                        }, (s) => s.ToRunOnceIn(2).Seconds()
                    );
                    break;
            }
        }
    }
}
