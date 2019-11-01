using Telebot.CoreApis;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
    {
        private readonly SystemApi systemApi;

        public UptimeStatus()
        {
            systemApi = ApiLocator.Instance.GetService<SystemApi>();
        }

        public string Execute()
        {
            return $"*Uptime*: {systemApi.GetUptime()}";
        }
    }
}
