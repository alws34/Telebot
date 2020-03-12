using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class WanAddrStatus : IStatus
    {
        private readonly NetworkImpl networkApi;

        public WanAddrStatus()
        {
            networkApi = new NetworkImpl();
        }

        public string GetStatus()
        {
            return $"*WAN IPv4*: {networkApi.WANIPv4}";
        }
    }
}
