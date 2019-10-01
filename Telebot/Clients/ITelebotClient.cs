using System;
using Telegram.Bot;

namespace Telebot.Clients
{
    public interface ITelebotClient : ITelegramBotClient
    {
        event EventHandler<RequestArrivalArgs> RequestArrival;
        int AdminId { get; }
    }
}
