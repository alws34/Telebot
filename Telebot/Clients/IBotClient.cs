using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telebot.Clients
{
    public interface IBotClient : ITelegramBotClient
    {
        event EventHandler<ReceivedArgs> Received;

        bool IsConnected { get; }

        void Connect();
        void Disconnect();

        Task SendText(string text, long chatId = 0, int replyId = 0);
        Task SendPic(Stream raw, long chatId = 0, int replyId = 0);
        Task SendDoc(Stream raw, long chatId = 0, int replyId = 0);
    }
}
