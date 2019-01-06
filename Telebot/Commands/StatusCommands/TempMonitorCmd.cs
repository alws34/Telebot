using Telebot.Contracts;
using Telebot.Controllers;

namespace Telebot.Commands.StatusCommands
{
    public class TempMonitorCmd : IStatusCommand
    {
        private readonly TempMonitorController controller;

        public TempMonitorCmd()
        {
            controller = Program.container.GetInstance<TempMonitorController>();
        }

        public string Execute()
        {
            return $"*Monitor (°C)*: {controller.GetMonitorStatus()}";
        }
    }
}
