using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class IPAddrStatus : IStatus
    {
        private readonly NetworkLogic networkLogic;

        public IPAddrStatus()
        {
            networkLogic = new NetworkLogic();
        }

        public string Execute()
        {
            return $"*IP*: {networkLogic.LocalIPv4Address}";
        }
    }
}
