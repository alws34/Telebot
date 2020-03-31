using Common.Enums;
using Common.Models;
using Contracts;
using Contracts.Factories;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace Telebot.Clients
{
    public class Telebot : IBotClient
    {
        private readonly IFactory<IPlugin> plugins;

        public Telebot(string token, int id) : base(token, id)
        {
            OnMessage += BotMessageHandler;

            plugins = Program.IocContainer.GetInstance<IFactory<IPlugin>>();
        }

        public async Task RespHandler(Response e)
        {
            switch (e.ResultType)
            {
                case ResultType.Text:
                    await SendText(e.Text, replyId: e.Reply ? e.MessageId : 0);
                    break;
                case ResultType.Photo:
                    await SendPic(e.Raw, replyId: e.Reply ? e.MessageId : 0);
                    break;
                case ResultType.Document:
                    await SendDoc(e.Raw, replyId: e.Reply ? e.MessageId : 0);
                    break;
            }
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message.From.Id != Id)
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

            bool success = plugins.TryGetEntity(
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

                plugin.Execute(req);

                return;
            }

            await SendText("Undefined command. For commands list, type */help*.", replyId: e.Message.MessageId);
        }
    }
}
