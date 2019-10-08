using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        string Pattern { get; }
        string Description { get; }
        void Execute(object parameter, Func<CommandResult, Task> callback);
        string ToString();
    }
}
