using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
    {
        private readonly SystemApi systemApi;

        public UptimeStatus()
        {
            systemApi = new SystemApi();
        }

        public string GetStatus()
        {
            return $"*Uptime*: {systemApi.GetUptime()}";
        }
    }
}
