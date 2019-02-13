using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        string Pattern { get; }
        string Description { get; }
        void Execute(object parameter, Action<CommandResult> callback);
        Task ExecuteAsync(object parameter, Action<CommandResult> callback);
        string ToString();
    }
}
