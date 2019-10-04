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

            foreach (ICommand command in commands)
            {
                _commands.Add(command);
            }
        }

        public ICommand FindEntity(Predicate<ICommand> predicate)
        {
            return _commands.SingleOrDefault(c => predicate(c));
        }

        public ICommand[] GetAllEntities()
        {
            return _commands.ToArray();
        }
    }
}
