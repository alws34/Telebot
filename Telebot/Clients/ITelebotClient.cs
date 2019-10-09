using System;
using Telegram.Bot;

namespace Telebot.Clients
{
    public interface ITelebotClient : ITelegramBotClient
    {
        event EventHandler<MessageArrivedArgs> MessageArrived;
        int AdminId { get; }
    }
}
