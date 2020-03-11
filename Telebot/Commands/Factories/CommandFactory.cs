using Telebot.Common;

namespace Telebot.Commands.Factories
{
    public class CommandFactory : Factory<ICommand>
    {
        public CommandFactory(ICommand[] commands)
        {
            _items.AddRange(commands);
        }
    }
}
