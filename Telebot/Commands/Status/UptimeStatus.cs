using Telebot.CoreApis;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
    {
        private readonly SystemApi systemLogic;

        public UptimeStatus()
        {
            systemLogic = new SystemApi();
        }

        public string Execute()
        {
            return $"*Uptime*: {systemLogic.GetUptime()}";
        }
    }
}
