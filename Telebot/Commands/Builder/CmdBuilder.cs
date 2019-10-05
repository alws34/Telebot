﻿using System.Collections.Generic;

namespace Telebot.Commands.Builder
{
    public class CmdBuilder
    {
        private readonly List<ICommand> commands;

        public CmdBuilder()
        {
            commands = new List<ICommand>();
        }

        public CmdBuilder Add(ICommand command)
        {
            commands.Add(command);
            return this;
        }

        public CmdBuilder AddRange(ICommand[] commands)
        {
            this.commands.AddRange(commands);
            return this;
        }

        public ICommand[] Build()
        {
            return commands.ToArray();
        }
    }
}