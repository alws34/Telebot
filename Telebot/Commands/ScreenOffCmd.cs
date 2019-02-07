using System;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class ScreenOffCmd : ICommand
    {
        public string Name => "/screen off";

        public string Description => "Turn off the display.";

        public event EventHandler<CommandResult> Completed;

        private readonly DisplayLogic screenLogic;

        public ScreenOffCmd()
        {
            screenLogic = Program.container.GetInstance<DisplayLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenLogic.SetDisplayOff();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Display will be turned off now.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);
        }

        public void ExecuteAsync(object parameter)
        {
            Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
