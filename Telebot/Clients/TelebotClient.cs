using System.IO;
using BotSdk.Enums;
using BotSdk.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using System;
using Telegram.Bot.Args;

namespace Telebot.Clients
{
    public class TelebotClient : TelegramBotClient, IBotClient
    {
        private readonly int Id;

        public event EventHandler<MessageEventArgs> Received
        {
            add => OnMessage += value;
            remove => OnMessage -= value;
        }

        public bool IsConnected => IsReceiving;

        public TelebotClient(string token, int id) : base(token)
        {
            Id = id;
        }

        public async Task ResultHandler(Response e)
        {
            switch (e.ResultType)
            {
                case ResultType.Text:
                    await SendText(e.Text, replyId: e.MessageId);
                    break;
                case ResultType.Photo:
                    await SendPic(e.Raw, replyId: e.MessageId);
                    break;
                case ResultType.Document:
                    await SendDoc(e.Raw, replyId: e.MessageId);
                    break;
            }
        }

        public async Task SendText(string text, long chatId = 0, int replyId = 0)
        {
            await SendTextMessageAsync(
                chatId == 0 ? Id : chatId,
                text.TrimEnd(),
                ParseMode.Markdown,
                replyToMessageId: replyId
            );
        }

        public async Task SendPic(Stream content, long chatId = 0, int replyId = 0)
        {
            var raw = new InputOnlineFile(content, "preview.jpg");

            await SendDocumentAsync(
                chatId == 0 ? Id : chatId,
                raw,
                parseMode: ParseMode.Markdown,
                replyToMessageId: replyId,
                caption: "From *Telebot*",
                thumb: raw as InputMedia
            );

            content.Close();
        }

        public async Task SendDoc(Stream content, long chatId = 0, int replyId = 0)
        {
            var raw = new InputOnlineFile(content, (content as FileStream).Name);

            await SendDocumentAsync(
                chatId == 0 ? Id : chatId,
                raw,
                parseMode: ParseMode.Markdown,
                replyToMessageId: replyId,
                caption: "From *Telebot*",
                thumb: raw as InputMedia
            );

            content.Close();
        }

        public Task<User> GetMeAsync()
        {
            return base.GetMeAsync();
        }

        public void Connect()
        {
            StartReceiving();
        }

        public void Disconnect()
        {
            StopReceiving();
        }

        public bool IsAuthorized(int id)
        {
            return Id.Equals(id);
        }
    }
}
