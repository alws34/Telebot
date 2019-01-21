using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : ICommand
    {
        public string Name => "/shutdown";

        public string Description => "Shuts down the host machine.";

        private readonly PowerController powerController;

        public event EventHandler<CommandResult> Completed;

        public ShutdownCmd()
        {
            powerController = Program.container.GetInstance<PowerController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            powerController.ShutdownWindows();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Shutting down the host machine...",
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
