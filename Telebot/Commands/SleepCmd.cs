using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SleepCmd : ICommand
    {
        public string Name => "/sleep";

        public string Description => "Suspends the host machine.";

        public event EventHandler<CommandResult> Completed;

        private readonly PowerLogic powerLogic;

        public SleepCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            powerLogic.SleepWindows();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Suspending the host machine...",
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
