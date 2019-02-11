using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.Commands.Facotries;
using Telebot.Events;
using Telebot.Models;
using Telegram.Bot.Types;

namespace Telebot.Commands.Dispatchers
{
    public class CommandDispatcher
    {
        public async Task<bool> Dispatch(string pattern, Message message)
        {
            var command = CommandFactory.Instance.GetCommand(pattern);

            if (command != null)
            {
                var groups = Regex.Match(pattern, command.Pattern).Groups;

                var cmdParams = new CommandParam
                {
                    Groups = groups
                };

                var result = await command.ExecuteAsync(cmdParams);

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
