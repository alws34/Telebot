using CPUID.Contracts;
using System.Collections.Generic;

namespace Telebot.Commands.Status.Builder
{
    public class StatusBuilder : IBuilder<IStatus>
    {
        private readonly List<IStatus> _items;

        public StatusBuilder()
        {
            _items = new List<IStatus>();
        }

        public IBuilder<IStatus> Add(IStatus item)
        {
            _items.Add(item);
            return this;
        }

        public IBuilder<IStatus> AddRange(IStatus[] items)
        {
            _items.AddRange(items);
            return this;
        }

        public IStatus[] Build()
        {
            return _items.ToArray();
        }
    }
}
