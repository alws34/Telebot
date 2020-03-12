using System;
using System.Drawing;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : ICommand
    {
        private readonly DesktopApi desktopApi;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";

            desktopApi = new DesktopApi();
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            var photos = desktopApi.CaptureDesktop();

            foreach (Bitmap photo in photos)
            {
                var result = new Response
                {
                    ResultType = ResultType.Photo,
                    Raw = photo.ToStream()
                };

                await cbResult(result);
            }
        }
    }
}
