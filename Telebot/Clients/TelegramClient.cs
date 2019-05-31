using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Telebot.Commands.Factories;
using Telebot.Events;
using Telebot.Extensions;
using Telebot.HwProviders;
using Telebot.Models;
using Telebot.Temperature;
using Telebot.ScreenCaptures;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Clients
{
    public class TelegramClient
    {
        private readonly int idAdmin;
        private readonly TelegramBotClient botClient;

        public static TelegramClient Instance { get; } = new TelegramClient();

        TelegramClient()
        {
            idAdmin = Program.appSettings.TelegramAdminId;
            string token = Program.appSettings.TelegramToken;

            botClient = new TelegramBotClient(token);

            botClient.OnMessage += BotMessageHandler;
            initTitle();
            doAdminHello();

            WarningTempMonitor.Instance.TemperatureChanged += PermanentTemperatureChanged;
            TimedTempMonitor.Instance.TemperatureChanged += ScheduledTemperatureChanged;
            TimedScreenCapture.Instance.PhotoCaptured += PhotoCaptured;
        }

        public void Start()
        {
            botClient.StartReceiving();
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

        private void PermanentTemperatureChanged(object sender, IEnumerable<IDeviceProvider> devices)
        {
            foreach (IDeviceProvider device in devices)
            {
                string deviceName = device.DeviceName;
                float temperature = device.GetTemperature();

                string text = $"*[WARNING] {deviceName}*: {temperature}°C\nFrom *Telebot*";

                botClient.SendTextMessageAsync(idAdmin, text, ParseMode.Markdown);
            }
        }

        private void ScheduledTemperatureChanged(object sender, IEnumerable<IDeviceProvider> devices)
        {
            var text = new StringBuilder();

            foreach (IDeviceProvider device in devices)
            {
                string deviceName = device.DeviceName;
                float temperature = device.GetTemperature();

                text.AppendLine($"*{deviceName}*: {temperature}°C");
            }

            text.AppendLine("\nFrom *Telebot*");

            botClient.SendTextMessageAsync(idAdmin, text.ToString(), ParseMode.Markdown);
        }

        private void PhotoCaptured(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Photo.ToStream(), "captime.jpg");
            botClient.SendDocumentAsync(idAdmin, document, thumb: document as InputMedia);
        }

        private async void initTitle()
        {
            string title = $" - ({(await botClient.GetMeAsync()).Username})";
            EventAggregator.Instance.Publish(new OnSetBotTitleArgs(title));
        }

        private void doAdminHello()
        {
            botClient.SendTextMessageAsync(idAdmin, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown);
        }

        private void BotMessageHandler(object sender, MessageEventArgs e)
        {
            void resultCallback(CommandResult result)
            {
                switch (result.SendType)
                {
                    case SendType.Text:
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, result.Text.TrimEnd(),
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        break;
                    case SendType.Photo:
                        var photo = new InputOnlineFile(result.Stream, "capture.jpg");
                        botClient.SendDocumentAsync(e.Message.Chat.Id, photo,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: photo as InputMedia);
                        break;
                    case SendType.Document:
                        var document = new InputOnlineFile(result.Stream, (result.Stream as FileStream).Name);
                        botClient.SendDocumentAsync(e.Message.Chat.Id, document,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: document as InputMedia);
                        break;
                }
            }

            if (e.Message.From.Id != idAdmin)
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
    }
}
