using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
    {
        private readonly SystemImpl systemApi;

        public UptimeStatus()
        {
            systemApi = new SystemImpl();
        }

        public string GetStatus()
        {
            return $"*Uptime*: {systemApi.GetUptime()}";
        }
    }
}
