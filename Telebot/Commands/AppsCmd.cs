using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Telebot.Contracts;
using Telebot.BusinessLogic;
using Telebot.Helpers;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class AppsCmd : ICommand
    {
        public string Name => "/apps";

        public string Description => "List of open applications.";

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
