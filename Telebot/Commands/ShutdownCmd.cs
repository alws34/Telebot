using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : CommandBase
    {
        private readonly PowerLogic powerLogic;

        public ShutdownCmd()
        {
            Pattern = "/shutdown";
            Description = "Shuts down the workstation.";
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public override CommandResult Execute(object parameter)
        {
            var result = new CommandResult
            {
                Text = "Shutting down the workstation.",
                SendType = SendType.Text
            };

            powerLogic.ShutdownWorkstation();

            return result;
        }
    }
}
