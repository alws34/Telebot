using System;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class RebootCmd : ICommand
    {
        public string Name => "/reboot";

        public string Description => "Reboots the workstation.";

        private readonly PowerLogic powerLogic;

        public event EventHandler<CommandResult> Completed;

        public RebootCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = "Rebooting the workstation.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);

            powerLogic.RestartWorkstation();
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
