using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        string Pattern { get; }
        string Description { get; }
        void Execute(CommandParam info, Func<CommandResult, Task> cbResult);
        string ToString();
    }
}
