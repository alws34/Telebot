using Telebot.BusinessLogic;
using Telebot.DeviceProviders;
using Telebot.StatusCommands;

namespace Telebot.Commands.StatusCommands
{
    public class SystemCmd : IStatusCommand
    {
        private readonly SystemLogic systemLogic;

        public SystemCmd(params IDeviceProvider[][] deviceProviders)
        {
            systemLogic = new SystemLogic
            (
                deviceProviders
            );
        }

        public string Execute()
        {
            return systemLogic.GetProvidersInfo();
        }
    }
}
