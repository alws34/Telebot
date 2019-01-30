using System;
using Telebot.BusinessLogic;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : ICommand
    {
        public string Name => "/shutdown";

        public string Description => "Shuts down the host machine.";

        private readonly PowerLogic powerLogic;

        public event EventHandler<CommandResult> Completed;

        public ShutdownCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            powerLogic.ShutdownWindows();

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
