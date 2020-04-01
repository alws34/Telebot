using Common.Contracts;
using CPUID.Base;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class SysInfo : IClassStatus
    {
        private readonly IEnumerable<IDevice> devices;

        public SysInfo(IEnumerable<IDevice> devices)
        {
            this.devices = devices;
        }

        public string GetStatus()
        {
            var builder = new StringBuilder();

            foreach (IDevice device in devices)
            {
                builder.AppendLine(device.ToString());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
