using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class LanAddrStatus : IStatus
    {
        private readonly NetworkImpl networkApi;

        public LanAddrStatus()
        {
            networkApi = new NetworkImpl();
        }

        public string GetStatus()
        {
            return $"*LAN IPv4*: {networkApi.LANIPv4}";
        }
    }
}
