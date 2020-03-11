using System.IO;
using System.Threading.Tasks;
using Telebot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class TransmitDocument : ITransmitter
    {
        protected readonly ITelebotClient client;

        public TransmitDocument(ITelebotClient client)
        {
            this.client = client;
        }

        public Task Transmit(CommandResult data)
        {
            var raw = new InputOnlineFile(data.Raw, (data.Raw as FileStream).Name);

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