using System;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        event EventHandler<CommandResult> Completed;
        void Execute(object parameter);
        void ExecuteAsync(object parameter);
    }
}
