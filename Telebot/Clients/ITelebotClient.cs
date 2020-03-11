using System;
using Telegram.Bot;

namespace Telebot.Clients
{
    public interface ITelebotClient : ITelegramBotClient
    {
        int AdminId { get; }

        event EventHandler<MessageArrivedArgs> MessageArrived;
    }
}
