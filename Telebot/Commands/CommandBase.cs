using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public abstract class CommandBase : ICommand
    {
        public string Pattern { get; protected set; }

        public string Description { get; protected set; }

        public abstract void Execute(object parameter, Action<CommandResult> callback);

        public Task ExecuteAsync(object parameter, Action<CommandResult> callback)
        {
            return Task.Run(() => Execute(parameter, callback));
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
