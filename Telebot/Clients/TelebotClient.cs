using System.Threading.Tasks;
using BotSdk.Enums;
using BotSdk.Models;

namespace Telebot.Clients
{
    public class TelebotClient : IBotClient
    {
        public TelebotClient(string token, int id) : base(token, id)
        {

        }

        public override async Task ResultHandler(Response e)
        {
            switch (e.ResultType)
            {
                case ResultType.Text:
                    await SendText(e.Text, replyId: e.MessageId);
                    break;
                case ResultType.Photo:
                    await SendPic(e.Raw, replyId: e.MessageId);
                    break;
                case ResultType.Document:
                    await SendDoc(e.Raw, replyId: e.MessageId);
                    break;
            }
        }
    }
}
