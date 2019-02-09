using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SleepCmd : ICommand
    {
        public string Pattern => "/sleep";

        public string Description => "Puts the workstation into sleep mode.";

        public event EventHandler<CommandResult> Completed;

        private readonly PowerLogic powerLogic;

        public SleepCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = "Workstation is entering sleep mode.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);

            powerLogic.SleepWorkstation();
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
