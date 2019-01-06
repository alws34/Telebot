using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Helpers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : ICommand
    {
        public string Name => "/apps";

        public string Description => "List of open applications.";

        public event EventHandler<CommandResult> Completed;

        private readonly WindowsController winController;

        public AppsCmd()
        {
            winController = Program.container.GetInstance<WindowsController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = winController.GetActiveApplications(),
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);
        }

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
