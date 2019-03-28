using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Telebot.Commands.Factories;
using Telebot.Events;
using Telebot.Extensions;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.ScreenCaptures;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Services
{
    public class TelegramService : ICommunicationService
    {
        private readonly string token;
        private readonly int adminId;
        private readonly TelegramBotClient client;

        public TelegramService()
        {
            token = Program.appSettings.TelegramToken;
            adminId = Program.appSettings.TelegramAdminId;

            if (!string.IsNullOrEmpty(token))
            {
                client = new TelegramBotClient(token);
                client.OnMessage += BotMessageHandler;
                initTitle();
                doAdminHello();
            }

            PermanentTempMonitor.Instance.TemperatureChanged += PermanentTemperatureChanged;
            ScheduledTempMonitor.Instance.TemperatureChanged += ScheduledTemperatureChanged;
            ScheduledScreenCapture.Instance.PhotoCaptured += PhotoCaptured;
        }

        private void PermanentTemperatureChanged(object sender, IEnumerable<HardwareInfo> devices)
        {
            foreach (HardwareInfo device in devices)
            {
                string text = $"*[WARNING] {device.DeviceName}*: {device.Value}°C\nFrom *Telebot*";

                client.SendTextMessageAsync(adminId, text, ParseMode.Markdown);
            }
        }

        private void ScheduledTemperatureChanged(object sender, IEnumerable<HardwareInfo> devices)
        {
            var text = new StringBuilder();

            foreach (HardwareInfo device in devices)
            {
                text.AppendLine($"*{device.DeviceName}*: {device.Value}°C");
            }

            text.AppendLine("\nFrom *Telebot*");

            client.SendTextMessageAsync(adminId, text.ToString(), ParseMode.Markdown);
        }

        private void PhotoCaptured(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Photo.ToStream(), "captime.jpg");
            client.SendDocumentAsync(adminId, document, thumb: document as InputMedia);
        }

        private async void initTitle()
        {
            string title = $" - ({(await client.GetMeAsync()).Username})";
            EventAggregator.Instance.Publish(new OnSetBotTitleArgs(title));
        }

        private void doAdminHello()
        {
            client.SendTextMessageAsync(adminId, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown);
        }

        private void BotMessageHandler(object sender, MessageEventArgs e)
        {
            void resultCallback(CommandResult result)
            {
                switch (result.SendType)
                {
                    case SendType.Text:
                        client.SendTextMessageAsync(e.Message.Chat.Id, result.Text.TrimEnd(),
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        break;
                    case SendType.Photo:
                        var photo = new InputOnlineFile(result.Stream, "capture.jpg");
                        client.SendDocumentAsync(e.Message.Chat.Id, photo,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: photo as InputMedia);
                        break;
                    case SendType.Document:
                        var document = new InputOnlineFile(result.Stream, (result.Stream as FileStream).Name);
                        client.SendDocumentAsync(e.Message.Chat.Id, document,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: document as InputMedia);
                        break;
                }
            }

            if (e.Message.From.Id != adminId)
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Unauthorized."
                };
                resultCallback(cmdResult);
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

                command.Execute(cmdParams, resultCallback);
            }
            else
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Undefined command. For commands list, type */help*."
                };
                resultCallback(cmdResult);
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
