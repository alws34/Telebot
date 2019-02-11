using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public interface ICommand
    {
        string Pattern { get; }
        string Description { get; }
        CommandResult Execute(object parameter);
        Task<CommandResult> ExecuteAsync(object parameter);
        string ToString();
    }
}
