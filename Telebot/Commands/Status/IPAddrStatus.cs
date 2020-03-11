using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class IPAddrStatus : IStatus
    {
        private readonly NetworkApi networkApi;

        public IPAddrStatus()
        {
            networkApi = new NetworkApi();
        }

        public string Execute()
        {
            return $"*IP*: {networkApi.LocalIPv4Address}";
        }
    }
}
