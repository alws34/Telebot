using System;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class AppsCmd : ICommand
    {
        public string Name => "/apps";

        public string Description => "List of active applications.";

        public event EventHandler<CommandResult> Completed;

        private readonly WindowsLogic windowsLogic;

        public AppsCmd()
        {
            windowsLogic = Program.container.GetInstance<WindowsLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = windowsLogic.GetActiveApplications(),
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
