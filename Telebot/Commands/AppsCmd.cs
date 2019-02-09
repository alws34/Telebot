using System;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class AppsCmd : ICommand
    {
        public string Pattern => "/apps";

        public string Description => "List of active applications.";

        public event EventHandler<CommandResult> Completed;

        private readonly WindowsLogic windowsLogic;

        public AppsCmd()
        {
            windowsLogic = Program.container.GetInstance<WindowsLogic>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = windowsLogic.GetActiveApplications(),
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
