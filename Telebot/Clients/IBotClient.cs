using System;
using BotSdk.Models;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public interface IBotClient
    {
        event EventHandler<MessageEventArgs> Received;

        Task ResultHandler(Response response);

        bool IsConnected { get; }

        void Connect();
        void Disconnect();
        bool IsAuthorized(int id);

        Task<User> GetMeAsync();

        Task SendText(string text, long chatId = 0, int replyId = 0);
        Task SendPic(Stream content, long chatId = 0, int replyId = 0); 
        Task SendDoc(Stream content, long chatId = 0, int replyId = 0);
    }
}
