using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
    {
        private readonly SystemLogic systemLogic;

        public UptimeStatus()
        {
            systemLogic = new SystemLogic();
        }

        public string Execute()
        {
            return $"*Uptime*: {systemLogic.GetUptime()}";
        }
    }
}
