using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class LanAddrStatus : IStatus
    {
        private readonly NetworkApi networkApi;

        public LanAddrStatus()
        {
            networkApi = new NetworkApi();
        }

        public string Execute()
        {
            return $"*LAN IPv4*: {networkApi.LANIPv4}";
        }
    }
}
