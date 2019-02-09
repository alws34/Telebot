using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOnCmd : ICommand
    {
        public string Pattern => "/screen on";

        public string Description => "Turn on the display.";

        public event EventHandler<CommandResult> Completed;

        private readonly DisplayLogic screenLogic;

        public ScreenOnCmd()
        {
            screenLogic = Program.container.GetInstance<DisplayLogic>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            screenLogic.SetDisplayOn();

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = "Display will be turned on now.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);
        }

        public void ExecuteAsync(object parameter)
        {
            Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
