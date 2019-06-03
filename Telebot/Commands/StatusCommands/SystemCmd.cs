using Telebot.BusinessLogic;
using Telebot.DeviceProviders;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class SystemCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public SystemCmd()
        {
            systemLogic = new SystemLogic
            (
                ProvidersFactory.GetRAMProviders(),
                ProvidersFactory.GetCPUProviders(),
                ProvidersFactory.GetDriveProviders(),
                ProvidersFactory.GetGPUProviders()
            );
        }

        public string Execute()
        {
            return systemLogic.GetSystemStatus();
        }
    }
}
