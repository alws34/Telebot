using CPUID.Contracts;
using System.Collections.Generic;

namespace CPUID.Builder
{
    public class DeviceBuilder : IBuilder<IDevice>
    {
        private readonly List<IDevice> _items;

        public DeviceBuilder()
        {
            _items = new List<IDevice>();
        }

        public IBuilder<IDevice> Add(IDevice item)
        {
            _items.Add(item);
            return this;
        }

        public IBuilder<IDevice> AddRange(IDevice[] items)
        {
            _items.AddRange(items);
            return this;
        }

        public IDevice[] Build()
        {
            return _items.ToArray();
        }
    }
}
