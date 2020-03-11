using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Clients
{
    public class Transmitable
    {
        private readonly Dictionary<ResultType, ITransmitter> methods;

        public Transmitable(IBotClient client)
        {
            methods = new Dictionary<ResultType, ITransmitter>
            {
                { ResultType.Text, new TransmitText(client) },
                { ResultType.Photo, new TransmitPhoto(client) },
                { ResultType.Document, new TransmitDocument(client) }
            };
        }

        public Task Transmit(CommandResult data)
        {
            return methods[data.ResultType].Transmit(data);
        }
    }
}
