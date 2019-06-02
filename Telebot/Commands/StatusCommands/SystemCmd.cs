using Telebot.BusinessLogic;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class SystemCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public SystemCmd()
        {
            systemLogic = new SystemLogic();
        }

        public string Execute()
        {
            return systemLogic.GetSystemStatus();
        }
    }
}
