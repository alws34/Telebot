using Telebot.Contracts;
using Telebot.Controllers;

namespace Telebot.Commands.StatusCommands
{
    public class SystemCmd : IStatusCommand
    {
        private readonly SystemController sysController;

        public SystemCmd()
        {
            sysController = Program.container.GetInstance<SystemController>();
        }

        public string Execute()
        {
            return sysController.GetSystemStatus();
        }
    }
}
