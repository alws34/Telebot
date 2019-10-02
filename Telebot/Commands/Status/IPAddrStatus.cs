using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class IPAddrStatus : IStatus
    {
        private readonly NetworkApi networkLogic;

        public IPAddrStatus()
        {
            networkLogic = new NetworkApi();
        }

        public string Execute()
        {
            return $"*IP*: {networkLogic.LocalIPv4Address}";
        }
    }
}
