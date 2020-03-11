using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public abstract class CommandBase : ICommand
    {
        public string Pattern { get; protected set; }

        public string Description { get; protected set; }

        public abstract void Execute(CommandParam info, Func<CommandResult, Task> cbResult);

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
