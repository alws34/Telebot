using System;
using Telebot.BusinessLogic;
using Telebot.Extensions;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : CommandBase
    {
        private readonly CaptureLogic captureLogic;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
            captureLogic = Program.container.GetInstance<CaptureLogic>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var bitmap = captureLogic.CaptureDesktop();

            var result = new CommandResult
            {
                Stream = bitmap.ToStream(),
                SendType = SendType.Photo
            };

            callback(result);
        }
    }
}
