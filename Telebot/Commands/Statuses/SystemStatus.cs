using Telebot.DeviceProviders;
using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class SystemStatus : IStatus
    {
        private readonly SystemLogic systemLogic;

        public SystemStatus(params IDeviceProvider[][] deviceProviders)
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
