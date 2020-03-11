using System.Threading.Tasks;
using Telebot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class TransmitPhoto : ITransmitter
    {
        protected readonly IBotClient client;

        public TransmitPhoto(IBotClient client)
        {
            this.client = client;
        }

        public Task Transmit(CommandResult data)
        {
            var raw = new InputOnlineFile(data.Raw, "capture.jpg");

            return client.SendDocumentAsync(
                data.ChatId,
                raw,
                parseMode: ParseMode.Markdown,
                replyToMessageId: data.MsgId,
                caption: "From *Telebot*",
                thumb: raw as InputMedia
            );
        }
    }
}