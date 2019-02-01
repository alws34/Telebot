using Telebot.BusinessLogic;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class UptimeCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public UptimeCmd()
        {
            systemLogic = Program.container.GetInstance<SystemLogic>();
        }

        public string Execute()
        {
            return $"*Uptime*: {systemLogic.GetUptime()}";
        }
    }
}
