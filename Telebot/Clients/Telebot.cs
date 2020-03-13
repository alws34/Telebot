using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.Commands;
using Telebot.Common;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class Telebot : TelegramBotClient, IBotClient
    {
        private readonly int id;

        public bool IsConnected => IsReceiving;

        public event EventHandler<ReceivedArgs> Received;

        public Telebot(string token, int id) : base(token)
        {
            this.id = id;

            OnMessage += BotMessageHandler;
        }

        public async Task SendText(string text, int replyId = 0)
        {
            await SendTextMessageAsync(
               id,
               text.TrimEnd(),
               parseMode: ParseMode.Markdown,
               replyToMessageId: replyId
           );
        }

        public async Task SendPic(Stream content, int replyId = 0)
        {
            var raw = new InputOnlineFile(content, "preview.jpg");

            await SendDocumentAsync(
                id,
                raw,
                parseMode: ParseMode.Markdown,
                replyToMessageId: replyId,
                caption: "From *Telebot*",
                thumb: raw as InputMedia
            );

            content.Close();
        }

        public async Task SendDoc(Stream content, int replyId = 0)
        {
            var raw = new InputOnlineFile(content, (content as FileStream).Name);

            await SendDocumentAsync(
                id,
                raw,
                parseMode: ParseMode.Markdown,
                replyToMessageId: replyId,
                caption: "From *Telebot*",
                thumb: raw as InputMedia
            );

            content.Close();
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async Task RespHandler(Response result)
            {
                switch (result.ResultType)
                {
                    case ResultType.Text:
                        await SendText(result.Text, e.Message.MessageId);
                        break;
                    case ResultType.Photo:
                        await SendPic(result.Raw, e.Message.MessageId);
                        break;
                    case ResultType.Document:
                        await SendDoc(result.Raw, e.Message.MessageId);
                        break;
                    default:
                        break;
                }
            }

            if (e.Message.From.Id != id)
            {
                await SendText("Unauthorized.", e.Message.MessageId);
                return;
            }

            string pattern = e.Message.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                await SendText("Unrecognized pattern.", e.Message.MessageId);
                return;
            }

            string text = $"Received {pattern} from {e.Message.From.Username}.";

            var data = new ReceivedArgs
            {
                MessageText = text
            };

            RaiseReceived(data);

            bool success = Program.CommandFactory.TryGetEntity(
                x => Regex.IsMatch(pattern, $"^{x.Pattern}$"),
                out ICommand command
            );

            if (success)
            {
                Match match = Regex.Match(pattern, command.Pattern);

                var req = new Request
                {
                    Groups = match.Groups
                };

                command.Execute(req, RespHandler);

                return;
            }

            await SendText("Undefined command. For commands list, type */help*.", e.Message.MessageId);
        }

        private void RaiseReceived(ReceivedArgs e)
        {
            Received?.Invoke(this, e);
        }

        public void Connect()
        {
            StartReceiving();
        }

        public void Disconnect()
        {
            StopReceiving();
        }
    }
}
