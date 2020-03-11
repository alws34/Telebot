using System;
using Telegram.Bot;

namespace Telebot.Clients
{
    public interface IBotClient : ITelegramBotClient
    {
        int AdminId { get; }

        event EventHandler<ReceivedArgs> Received;
    }
}
