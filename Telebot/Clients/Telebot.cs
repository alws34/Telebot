﻿using Common.Enums;
using Common.Models;
using Contracts;
using Contracts.Factories;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace Telebot.Clients
{
    public class TeleBot : IBotClient
    {
        private readonly IFactory<IPlugin> pluginFac;

        public TeleBot(string token, int id) : base(token, id)
        {
            OnMessage += BotMessageHandler;

            pluginFac = Program.IocContainer.GetInstance<IFactory<IPlugin>>();
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

            bool success = pluginFac.TryGetEntity(
                x => Regex.IsMatch(pattern, $"^{x.Pattern}$"),
                out IPlugin plugin
            );

            if (success)
            {
                Match match = Regex.Match(pattern, plugin.Pattern);

                var req = new Request
                {
                    Groups = match.Groups,
                    MessageId = e.Message.MessageId
                };

                plugin.Execute(req);

                return;
            }

            await SendText("Undefined command. For commands list, type */help*.", replyId: e.Message.MessageId);
        }
    }
}
