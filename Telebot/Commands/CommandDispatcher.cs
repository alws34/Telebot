using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                command.Completed += CommandCompleted;
            }
        }

        public bool Dispatch(string pattern, Message message)
        {
            var command = _commands.SingleOrDefault(x => x.Key.IsMatch(pattern));

            if (command.Key != null)
            {
                var cmdArgs = new CommandParam
                {
                    Message = message,
                    Commands = _commands.Values.ToArray(),
                    Parameters = command.Key.Match(pattern)
                };

                command.Value.ExecuteAsync(cmdArgs);

                return true;
            }

            return false;
        }

        private void CommandCompleted(object sender, CommandResult e)
        {
            switch (e.SendType)
            {
                case SendType.Text:
                    EventAggregator.Instance.Publish(new OnSendTextToChatArgs(
                        e.Text.TrimEnd(), e.Message.Chat.Id, e.Message.MessageId));
                    break;
                case SendType.Photo:
                    EventAggregator.Instance.Publish(new OnSendPhotoToChatArgs(
                       e.Stream, e.Message.Chat.Id, e.Message.MessageId));
                    break;
            }
        }
    }
}
