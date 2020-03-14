using Common;
using System.Collections.Generic;

namespace Telebot.Commands.Factories
{
    public class CommandFactory : IFactory<ICommand>
    {
        public CommandFactory(IEnumerable<ICommand> commands)
        {
            _items.AddRange(commands);
        }
    }
}
