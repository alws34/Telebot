using System;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        event EventHandler<CommandResult> Completed;
        string Pattern { get; }
        string Description { get; }
        void Execute(object parameter);
        void ExecuteAsync(object parameter);
        string ToString();
    }
}
