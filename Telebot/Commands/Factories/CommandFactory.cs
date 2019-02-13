using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Telebot.Commands.Factories
{
    public sealed class CommandFactory
    {
        private readonly Dictionary<Regex, ICommand> _commands;

        public static CommandFactory Instance { get; } = new CommandFactory();

        CommandFactory()
        {
            _commands = new Dictionary<Regex, ICommand>();

            var commands = Program.container.GetAllInstances<ICommand>();

            foreach (ICommand command in commands)
            {
                _commands.Add(new Regex($"^{command.Pattern}$"), command);
            }
        }

        public ICommand GetCommand(string pattern)
        {
            return _commands.SingleOrDefault(x => x.Key.IsMatch(pattern)).Value;
        }

        public ICommand[] GetAllCommands()
        {
            return _commands.Values.ToArray();
        }
    }
}
