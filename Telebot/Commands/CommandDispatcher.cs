using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.Events;
using Telebot.Models;
using Telegram.Bot.Types;

namespace Telebot.Commands
{
    public class CommandDispatcher
    {
        private readonly Dictionary<Regex, ICommand> _commands;

        public CommandDispatcher()
        {
            _commands = new Dictionary<Regex, ICommand>();

            var commands = Program.container.GetAllInstances<ICommand>();

            foreach (ICommand command in commands)
            {
                _commands.Add(new Regex($"^{command.Pattern}$"), command);
            }
        }

        public async Task<bool> Dispatch(string pattern, Message message)
        {
            var command = _commands.SingleOrDefault(x => x.Key.IsMatch(pattern));

            if (command.Key != null)
            {
                var cmdArgs = new CommandParam
                {
                    Commands = _commands.Values.ToArray(),
                    Groups = command.Key.Match(pattern).Groups
                };

                var result = await command.Value.ExecuteAsync(cmdArgs);

                switch (result.SendType)
                {
                    case SendType.Text:
                        EventAggregator.Instance.Publish(new OnSendTextToChatArgs(
                            result.Text.TrimEnd(), message.Chat.Id, message.MessageId));
                        break;
                    case SendType.Photo:
                        EventAggregator.Instance.Publish(new OnSendPhotoToChatArgs(
                           result.Stream, message.Chat.Id, message.MessageId));
                        break;
                }

                return true;
            }

            return false;
        }
    }
}
