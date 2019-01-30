using Telebot.Contracts;
using Telebot.BusinessLogic;

namespace Telebot.Commands.StatusCommands
{
    public class SystemCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public SystemCmd()
        {
            systemLogic = Program.container.GetInstance<SystemLogic>();
        }

        public string Execute()
        {
            return systemLogic.GetSystemStatus();
        }
    }
}
