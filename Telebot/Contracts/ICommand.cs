using System;
using Telebot.Models;
using Telegram.Bot.Types;

namespace Telebot.Contracts
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        event EventHandler<CommandResult> Completed;
        void Execute(object parameter);
    }
}
