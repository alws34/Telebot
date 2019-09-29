using CPUID.Contracts;
using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class SystemStatus : IStatus
    {
        private readonly SystemLogic systemLogic;

        public SystemStatus(params IDevice[][] devices)
        {
            systemLogic = new SystemLogic
            (
                devices
            );
        }

        public string Execute()
        {
            return systemLogic.GetDevicesInfo();
        }
    }
}
