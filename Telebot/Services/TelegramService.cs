using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telebot.Commands;
using Telebot.Events;
using Telebot.Extensions;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Views;
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

        private readonly CommandDispatcher cmdDispatcher;

        public TelegramService()
        {
            settings = Program.container.GetInstance<ISettings>();
            cmdDispatcher = Program.container.GetInstance<CommandDispatcher>();

            LoadSettings();

            if (!string.IsNullOrEmpty(token))
            {
                client = new TelegramBotClient(token);
                client.OnMessage += BotMessageHandler;
                doStartup();
            }

            EventAggregator.Instance.Subscribe<OnSendPhotoToChatArgs>(OnSendPhotoToChat);
            EventAggregator.Instance.Subscribe<OnSendTextToChatArgs>(OnSendTextToChat);
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

        private void OnSendTextToChat(OnSendTextToChatArgs obj)
        {
            client.SendTextMessageAsync(obj.ChatId, 
                obj.Text, parseMode: ParseMode.Markdown, replyToMessageId: obj.MessageId);
        }

        private void OnSendPhotoToChat(OnSendPhotoToChatArgs obj)
        {
            client.SendPhotoAsync(obj.ChatId,
                obj.PhotoStream, parseMode: ParseMode.Markdown, replyToMessageId: obj.MessageId);
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

        private void BotMessageHandler(object sender, MessageEventArgs e)
        {
            if (!whitelist.Exists(x => x.Equals(e.Message.From.Id)))
            {
                var cmdResult = new OnSendTextToChatArgs("Unauthorized.", e.Message.Chat.Id, 0);
                OnSendTextToChat(cmdResult);
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

            if (!cmdDispatcher.Dispatch(cmdPattern, e.Message))
            {
                var cmdResult = new OnSendTextToChatArgs(
                    "Undefined command. For commands list, type */help*.", e.Message.Chat.Id, 0);
                OnSendTextToChat(cmdResult);
            }
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
