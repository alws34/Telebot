using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOffCmd : CommandBase
    {
        private readonly DisplayLogic screenLogic;

        public ScreenOffCmd()
        {
            Pattern = "/screen off";
            Description = "Turn off the display.";
            screenLogic = Program.container.GetInstance<DisplayLogic>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Display will be turned off now.",
                SendType = SendType.Text
            };

            callback(result);

            screenLogic.SetDisplayOff();
        }
    }
}
