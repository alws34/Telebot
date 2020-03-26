using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telebot.Clients
{
    public abstract class IBotClient : TelegramBotClient
    {
        public event EventHandler<NotificationArgs> Notification;

        protected void RaiseNotification(NotificationArgs e)
        {
            Notification?.Invoke(this, e);
        }

        public IBotClient(string token) : base(token)
        {

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

        public abstract Task SendText(string text, long chatId = 0, int replyId = 0);
        public abstract Task SendPic(Stream raw, long chatId = 0, int replyId = 0);
        public abstract Task SendDoc(Stream raw, long chatId = 0, int replyId = 0);
    }
}
