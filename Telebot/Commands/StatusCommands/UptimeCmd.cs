using Telebot.Infrastructure;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class UptimeCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public UptimeCmd()
        {
            systemLogic = new SystemLogic();
        }

        public string Execute()
        {
            return $"*Uptime*: {systemLogic.GetUptime()}";
        }
    }
}
