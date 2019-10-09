using CPUID.Contracts;
using System.Collections.Generic;

namespace Telebot.Commands.Builder
{
    public class CmdBuilder : IBuilder<ICommand>
    {
        private readonly List<ICommand> _items;

        public CmdBuilder()
        {
            _items = new List<ICommand>();
        }

        public IBuilder<ICommand> Add(ICommand item)
        {
            _items.Add(item);
            return this;
        }

        public IBuilder<ICommand> AddRange(ICommand[] items)
        {
            this._items.AddRange(items);
            return this;
        }

        public ICommand[] Build()
        {
            return _items.ToArray();
        }
    }
}
