using System;
using System.Threading.Tasks;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SleepCmd : ICommand
    {
        public string Name => "/sleep";

        public string Description => "Puts the workstation into sleep mode.";

        public event EventHandler<CommandResult> Completed;

        private readonly PowerLogic powerLogic;

        public SleepCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Workstation is entering sleep mode.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);

            powerLogic.SleepWorkstation();
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
