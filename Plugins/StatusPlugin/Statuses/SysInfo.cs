using Common;
using CPUID.Base;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class SysInfo : IStatus
    {
        private readonly IEnumerable<IDevice> devices;

        public SysInfo(IEnumerable<IDevice> devices)
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
