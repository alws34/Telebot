using CPUID.Base;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class SystemStatus : IStatus
    {
        private readonly IEnumerable<IDevice> devices;

        public SystemStatus(IEnumerable<IDevice> devices)
        {
            this.devices = devices;
        }

        public string GetStatus()
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
