using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : ICommand
    {
        public string Name => "/shutdown";

        public string Description => "Shuts down the workstation.";

        private readonly PowerLogic powerLogic;

        public event EventHandler<CommandResult> Completed;

        public ShutdownCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Shutting down the workstation.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);

            powerLogic.ShutdownWorkstation();
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
