using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOnCmd : CommandBase
    {
        private readonly DisplayLogic screenLogic;

        public ScreenOnCmd()
        {
            Pattern = "/screen on";
            Description = "Turn on the display.";
            screenLogic = Program.container.GetInstance<DisplayLogic>();
        }

        public override CommandResult Execute(object parameter)
        {
            screenLogic.SetDisplayOn();

            var result = new CommandResult
            {
                Text = "Display will be turned on now.",
                SendType = SendType.Text
            };

            return result;
        }
    }
}
