using Telebot.BusinessLogic;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class IPCmd : IStatusCommand
    {
        private readonly NetworkLogic networkLogic;

        public IPCmd()
        {
            networkLogic = Program.container.GetInstance<NetworkLogic>();
        }

        public string Execute()
        {
            return $"*IP*: {networkLogic.IP}";
        }
    }
}
