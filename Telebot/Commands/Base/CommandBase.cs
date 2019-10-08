using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public abstract class CommandBase : ICommand
    {
        public string Pattern { get; protected set; }

        public string Description { get; protected set; }

        public abstract void Execute(object parameter, Func<CommandResult, Task> callback);

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
