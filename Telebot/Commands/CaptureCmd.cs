using System;
using System.Drawing;
using System.Threading.Tasks;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : BaseCommand
    {
        private readonly DesktopApi desktopApi;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";

            desktopApi = ApiLocator.Instance.GetService<DesktopApi>();
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            var photos = desktopApi.CaptureDesktop();

            foreach (Bitmap photo in photos)
            {
                var result = new CommandResult
                {
                    ResultType = ResultType.Photo,
                    Raw = photo.ToStream()
                };

                await cbResult(result);
            }
        }
    }
}
