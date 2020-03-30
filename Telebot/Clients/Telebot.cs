using Contracts;
using Common.Enums;
using Common.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telebot.NSPlugins;
using Telegram.Bot.Args;

namespace Telebot.Clients
{
    public class Telebot : IBotClient
    {
        public Telebot(string token, int id) : base(token, id)
        {
            OnMessage += BotMessageHandler;
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async Task RespHandler(Response result)
            {
                switch (result.ResultType)
                {
                    case ResultType.Text:
                        await SendText(result.Text, replyId: result.Reply ? e.Message.MessageId : 0);
                        break;
                    case ResultType.Photo:
                        await SendPic(result.Raw, replyId: result.Reply ? e.Message.MessageId : 0);
                        break;
                    case ResultType.Document:
                        await SendDoc(result.Raw, replyId: result.Reply ? e.Message.MessageId : 0);
                        break;
                    default:
                        break;
                }
            }

            if (e.Message.From.Id != id)
            {
                await SendText("Unauthorized.", e.Message.From.Id, e.Message.MessageId);
                return;
            }

            string pattern = e.Message.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                await SendText("Unrecognized pattern.", replyId: e.Message.MessageId);
                return;
            }

            string text = $"Received {pattern} from {e.Message.From.Username}.";

            var data = new NotificationArgs
            {
                NotificationText = text
            };

            RaiseNotification(data);

            bool success = Plugins.Instance.TryGetEntity(
                x => Regex.IsMatch(pattern, $"^{x.Pattern}$"),
                out IPlugin plugin
            );

            if (success)
            {
                Match match = Regex.Match(pattern, plugin.Pattern);

                var req = new Request
                {
                    Groups = match.Groups
                };

                plugin.Execute(req, RespHandler);

                return;
            }

            await SendText("Undefined command. For commands list, type */help*.", replyId: e.Message.MessageId);
        }
    }
}
