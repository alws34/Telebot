using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Telebot.Commands.Facotries;
using Telebot.Events;
using Telebot.Extensions;
using Telebot.Managers;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Telebot.Services
{
    public class TelegramService : ICommunicationService
    {
        private string token;
        private List<long> whitelist;

        private readonly TelegramBotClient client;
        private readonly ISettings settings;

        public TelegramService()
        {
            settings = Program.container.GetInstance<ISettings>();

            LoadSettings();

            if (!string.IsNullOrEmpty(token))
            {
                client = new TelegramBotClient(token);
                client.OnMessage += BotMessageHandler;
                doStartup();
            }

            EventAggregator.Instance.Subscribe<OnHighTemperatureArgs>(OnHighTemperature);
            EventAggregator.Instance.Subscribe<OnScreenCaptureArgs>(OnScreenCapture);
        }

        private void LoadSettings()
        {
            token = settings.TelegramToken;
            whitelist = settings.TelegramWhitelist;
        }

        private async void doStartup()
        {
            string title = $" - ({(await client.GetMeAsync()).Username})";
            EventAggregator.Instance.Publish(new OnSetBotTitleArgs(title));

            foreach (long chatid in whitelist)
            {
                client.SendTextMessageAsync(chatid, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown);
            }
        }

        private void OnHighTemperature(OnHighTemperatureArgs obj)
        {
            foreach (long chatid in whitelist)
            {
                client.SendTextMessageAsync(chatid, obj.Message, parseMode: ParseMode.Markdown);
            }
        }

        private void OnScreenCapture(OnScreenCaptureArgs obj)
        {
            foreach (long chatid in whitelist)
            {
                client.SendPhotoAsync(chatid, obj.Photo.ToStream(),
                    parseMode: ParseMode.Markdown, caption: "From *Telebot*");
            }
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            if (!whitelist.Exists(x => x.Equals(e.Message.From.Id)))
            {
                SendTextToChat("Unauthorized.", e.Message.Chat.Id, 0);
                return;
            }

            string cmdPattern = e.Message.Text;

            if (string.IsNullOrEmpty(cmdPattern))
            {
                return;
            }

            string info = $"Received {cmdPattern} from {e.Message.From.Username}.";

            var item = new LvItem
            {
                DateTime = DateTime.Now.ToString(),
                Text = info
            };

            EventAggregator.Instance.Publish(new OnAddObjectToLvArgs(item));

            EventAggregator.Instance.Publish(new OnNotifyIconBalloonArgs(info));

            var command = CommandFactory.Instance.GetCommand(cmdPattern);

            if (command != null)
            {
                var groups = Regex.Match(cmdPattern, command.Pattern).Groups;

                var cmdParams = new CommandParam
                {
                    Groups = groups
                };

                var result = await command.ExecuteAsync(cmdParams);

                switch (result.SendType)
                {
                    case SendType.Text:
                        SendTextToChat(result.Text.TrimEnd(), e.Message.Chat.Id, e.Message.MessageId);
                        break;
                    case SendType.Photo:
                        SendPhotoToChat(result.Stream, e.Message.Chat.Id, e.Message.MessageId);
                        break;
                }
            }
            else
            {
                string s = "Undefined command. For commands list, type */help*.";
                SendTextToChat(s, e.Message.Chat.Id, 0);
            }
        }

        private void SendTextToChat(string text, long chatid, int messageid)
        {
            client.SendTextMessageAsync(chatid, text, parseMode: ParseMode.Markdown, replyToMessageId: messageid);
        }

        private void SendPhotoToChat(Stream photoStream, long chatid, int messageid)
        {
            client.SendPhotoAsync(chatid, photoStream, parseMode: ParseMode.Markdown, replyToMessageId: messageid);
        }

        public void Start()
        {
            if (client != null && !client.IsReceiving)
            {
                client.StartReceiving();
            }
        }

        public void Stop()
        {
            if (client != null && client.IsReceiving)
            {
                client.StopReceiving();
            }
        }
    }
}
