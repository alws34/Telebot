using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.Commands;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telebot.Clients
{
    public class TelebotClient : TelegramBotClient, ITelebotClient
    {
        public int AdminId { get; }
        protected readonly Transmitable transmitable;

        public event EventHandler<MessageArrivedArgs> MessageArrived;

        public TelebotClient(string token, int id) : base(token)
        {
            AdminId = id;
            transmitable = new Transmitable(this);

            OnMessage += BotMessageHandler;
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async Task CallbackHandler(CommandResult result)
            {
                result.ChatId = e.Message.Chat.Id;
                result.FromId = e.Message.From.Id;
                result.MsgId = e.Message.MessageId;

                await transmitable.Transmit(result);
            }

            if (e.Message.From.Id != AdminId)
            {
                var result = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Unauthorized.",
                    ChatId = e.Message.Chat.Id,
                    FromId = e.Message.From.Id,
                    MsgId = e.Message.MessageId
                };

                await transmitable.Transmit(result);

                return;
            }

            string pattern = e.Message.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                var result = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Command pattern is null or empty.",
                    ChatId = e.Message.Chat.Id,
                    FromId = e.Message.From.Id,
                    MsgId = e.Message.MessageId
                };

                await transmitable.Transmit(result);

                return;
            }

            string text = $"Received {pattern} from {e.Message.From.Username}.";

            var data = new MessageArrivedArgs
            {
                MessageText = text
            };

            RaiseMessageArrived(data);

            ICommand command = Program.CmdFactory.FindEntity(
                x => Regex.IsMatch(pattern, $"^{x.Pattern}$")
            );

            if (command != null)
            {
                Match match = Regex.Match(pattern, command.Pattern);

                var cmdArgs = new CommandParam
                {
                    Groups = match.Groups
                };

                command.Execute(cmdArgs, CallbackHandler);
            }
            else
            {
                var result = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Undefined command. For commands list, type */help*.",
                    ChatId = e.Message.Chat.Id,
                    FromId = e.Message.From.Id,
                    MsgId = e.Message.MessageId
                };

                await transmitable.Transmit(result);
            }
        }

        private void RaiseMessageArrived(MessageArrivedArgs e)
        {
            MessageArrived?.Invoke(this, e);
        }
    }
}
