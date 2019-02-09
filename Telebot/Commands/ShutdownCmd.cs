using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : ICommand
    {
        public string Pattern => "/shutdown";

        public string Description => "Shuts down the workstation.";

        private readonly PowerLogic powerLogic;

        public event EventHandler<CommandResult> Completed;

        public ShutdownCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = "Shutting down the workstation.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);

            powerLogic.ShutdownWorkstation();
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
