using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOnCmd : ICommand
    {
        public string Name => "/screen on";

        public string Description => "Turn on the display.";

        public event EventHandler<CommandResult> Completed;

        private readonly DisplayLogic screenLogic;

        public ScreenOnCmd()
        {
            screenLogic = Program.container.GetInstance<DisplayLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenLogic.SetDisplayOn();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Display will be turned on now.",
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
