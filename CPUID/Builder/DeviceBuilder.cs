using CPUID.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUID.Builder
{
    public class DeviceBuilder
    {
        private readonly List<IDevice> devices;

        public DeviceBuilder()
        {
            devices = new List<IDevice>();
        }

        public DeviceBuilder AddItem(IDevice item)
        {
            devices.Add(item);
            return this;
        }

        public DeviceBuilder AddItems(IDevice[] items)
        {
            devices.AddRange(items);
            return this;
        }

        public IDevice[] Build()
        {
            return devices.ToArray();
        }
    }
}
