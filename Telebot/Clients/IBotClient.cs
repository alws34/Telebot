using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public abstract class IBotClient : TelegramBotClient
    {
        protected readonly int Id;

        protected IBotClient(string token, int id) : base(token)
        {
            Id = id;
        }

        public bool IsConnected => IsReceiving;

        public void Connect()
        {
            StartReceiving();
        }

        public void Disconnect()
        {
            StopReceiving();
        }

        public event EventHandler<NotificationArgs> Notification;
        protected void RaiseNotification(NotificationArgs e)
        {
            Notification?.Invoke(this, e);
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
    }
}
