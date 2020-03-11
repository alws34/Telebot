using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;

namespace Telebot.Commands.Factories
{
    public class CommandFactory : IFactory<ICommand>
    {
        private readonly List<ICommand> _commands;

        public CommandFactory(ICommand[] commands)
        {
            _commands = new List<ICommand>();

            _commands.AddRange(commands);
        }

        public ICommand FindEntity(Predicate<ICommand> predicate)
        {
            return _commands.Find(c => predicate(c));
        }

        public ICommand[] GetAllEntities()
        {
            return _commands.ToArray();
        }

        public bool TryGetEntity(Predicate<ICommand> predicate, out ICommand entity)
        {
            entity = _commands.Find(c => predicate(c));
            return entity != null;
        }
    }
}
