using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public abstract class CommandBase : ICommand
    {
        public string Pattern { get; protected set; }

        public string Description { get; protected set; }

        public abstract CommandResult Execute(object parameter);

        public async Task<CommandResult> ExecuteAsync(object parameter)
        {
            return await Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
