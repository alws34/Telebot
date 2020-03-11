using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.Commands;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class TelebotClient : TelegramBotClient, ITelebotClient
    {
        public int AdminId { get; }

        public event EventHandler<MessageArrivedArgs> MessageArrived;

        public TelebotClient(string token, int id) : base(token)
        {
            AdminId = id;

            OnMessage += BotMessageHandler;
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async Task executeCallback(CommandResult result)
            {
                switch (result.ResultType)
                {
                    case ResultType.Text:
                        await SendTextMessageAsync(e.Message.Chat.Id, result.Text.TrimEnd(),
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        break;
                    case ResultType.Photo:
                        var photo = new InputOnlineFile(result.Stream, "capture.jpg");
                        await SendDocumentAsync(e.Message.Chat.Id, photo,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: photo as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
                        break;
                    case ResultType.Document:
                        var document = new InputOnlineFile(result.Stream, (result.Stream as FileStream).Name);
                        await SendDocumentAsync(e.Message.Chat.Id, document,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: document as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
                        break;
                }
            }

            if (e.Message.From.Id != AdminId)
            {
                var cmdResult = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Unauthorized."
                };
                await executeCallback(cmdResult);
                return;
            }

            string pattern = e.Message.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                var cmdResult = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Command pattern is null or empty."
                };
                await executeCallback(cmdResult);
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

                command.Execute(cmdArgs, executeCallback);
            }
            else
            {
                var cmdResult = new CommandResult
                {
                    ResultType = ResultType.Text,
                    Text = "Undefined command. For commands list, type */help*."
                };
                await executeCallback(cmdResult);
            }
        }

        private void RaiseMessageArrived(MessageArrivedArgs e)
        {
            MessageArrived?.Invoke(this, e);
        }
    }
}
