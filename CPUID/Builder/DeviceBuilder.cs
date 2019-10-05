using CPUID.Contracts;
using System.Collections.Generic;

namespace CPUID.Builder
{
    public class DeviceBuilder
    {
        private readonly List<IDevice> devices;

        public DeviceBuilder()
        {
            devices = new List<IDevice>();
        }

        public DeviceBuilder Add(IDevice device)
        {
            devices.Add(device);
            return this;
        }

        public DeviceBuilder AddRange(IDevice[] devices)
        {
            this.devices.AddRange(devices);
            return this;
        }

        public IDevice[] Build()
        {
            return devices.ToArray();
        }
    }
}
