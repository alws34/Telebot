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

        public override CommandResult Execute(object parameter)
        {
            var bitmap = captureLogic.CaptureDesktop();

            var result = new CommandResult
            {
                Stream = bitmap.ToStream(),
                SendType = SendType.Photo
            };

            return result;
        }
    }
}
