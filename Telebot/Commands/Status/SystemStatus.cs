using CPUID.Contracts;
using System.Collections.Generic;
using System.Text;

namespace Telebot.Commands.Status
{
    public class SystemStatus : IStatus
    {
        private readonly List<IDevice> devices;

        public SystemStatus(IDevice[] devices)
        {
            this.devices = new List<IDevice>();

            this.devices.AddRange(devices);
        }

        public string Execute()
        {
            var strBuilder = new StringBuilder();

            foreach (IDevice device in devices)
            {
                strBuilder.AppendLine(device.ToString());
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}
