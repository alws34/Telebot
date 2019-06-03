using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Telebot.Commands.Factories;
using Telebot.DeviceProviders;
using Telebot.Events;
using Telebot.Extensions;
using Telebot.Models;
using Telebot.ScreenCaptures;
using Telebot.Temperature;
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

        private async void PermanentTemperatureChanged(object sender, IEnumerable<IDeviceProvider> devices)
        {
            foreach (IDeviceProvider device in devices)
            {
                string deviceName = device.DeviceName;
                SensorInfo package = device.GetTemperatureSensors().ElementAt(0);
                float temperature = package.Value;

                string text = $"*[WARNING] {deviceName}*: {temperature}°C\nFrom *Telebot*";

                await botClient.SendTextMessageAsync(idAdmin, text, ParseMode.Markdown);
            }
        }

        private async void ScheduledTemperatureChanged(object sender, IEnumerable<IDeviceProvider> devices)
        {
            var text = new StringBuilder();

            foreach (IDeviceProvider device in devices)
            {
                string deviceName = device.DeviceName;
                SensorInfo package = device.GetTemperatureSensors().ElementAt(0);
                float temperature = package.Value;

                text.AppendLine($"*{deviceName}*: {temperature}°C");
            }

            text.AppendLine("\nFrom *Telebot*");

            await botClient.SendTextMessageAsync(idAdmin, text.ToString(), ParseMode.Markdown);
        }

        private async void PhotoCaptured(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Photo.ToStream(), "captime.jpg");
            await botClient.SendDocumentAsync(idAdmin, document, thumb: document as InputMedia);
        }

        private async void initTitle()
        {
            string title = $" - ({(await botClient.GetMeAsync()).Username})";
            EventAggregator.Instance.Publish(new OnSetBotTitleArgs(title));
        }

        private async void doAdminHello()
        {
            await botClient.SendTextMessageAsync(idAdmin, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown);
        }

        private async void BotMessageHandler(object sender, MessageEventArgs e)
        {
            async void executeCallback(CommandResult result)
            {
                switch (result.SendType)
                {
                    case SendType.Text:
                        await botClient.SendTextMessageAsync(e.Message.Chat.Id, result.Text.TrimEnd(),
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        break;
                    case SendType.Photo:
                        var photo = new InputOnlineFile(result.Stream, "capture.jpg");
                        await botClient.SendDocumentAsync(e.Message.Chat.Id, photo,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: photo as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
                        break;
                    case SendType.Document:
                        var document = new InputOnlineFile(result.Stream, (result.Stream as FileStream).Name);
                        await botClient.SendDocumentAsync(e.Message.Chat.Id, document,
                            parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId,
                            caption: "From *Telebot*", thumb: document as InputMedia);
                        result.Stream.Close();
                        result.Stream.Dispose();
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
                executeCallback(cmdResult);
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

            var command = Program.commandFactory.GetCommand(cmdPattern);

            if (command != null)
            {
                var groups = Regex.Match(cmdPattern, command.Pattern).Groups;

                var cmdParams = new CommandParam
                {
                    Groups = groups
                };

                await command.ExecuteAsync(cmdParams, executeCallback);
            }
            else
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Undefined command. For commands list, type */help*."
                };
                executeCallback(cmdResult);
            }
        }
    }
}
