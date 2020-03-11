using Common;

namespace Telebot.Commands.Factories
{
    public class CommandFactory : IFactory<ICommand>
    {
        public CommandFactory(ICommand[] commands)
        {
            _items.AddRange(commands);
        }
    }
}
