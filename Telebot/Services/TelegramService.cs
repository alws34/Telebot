using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telebot.Commands;
using Telebot.Events;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Views;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot.Services
{
    public class TelegramService : ICommunicationService
    {
        private string token;
        private ChatId chatId;
        private List<int> whiteList;

        private readonly TelegramBotClient client;
        private readonly ISettings settings;
        private readonly IMainFormView mainFormView;

        private Dictionary<string, ICommand> commands;

        public TelegramService()
        {
            mainFormView = Program.container.GetInstance<MainForm>();

            this.commands = new Dictionary<string, ICommand>();

            var commands = Program.container.GetAllInstances<ICommand>();
            foreach (ICommand command in commands)
            {
                this.commands.Add(command.Name, command);
                command.Completed += CommandCompleted;
            }

            settings = Program.container.GetInstance<ISettings>();

            LoadSettings();

            if (!string.IsNullOrEmpty(token))
            {
                client = new TelegramBotClient(token);
                client.OnMessage += BotMessageHandler;
                doStartup();
            }

            EventAggregator.Instance.Subscribe<HighTemperatureMessage>(OnHighTemperature);
        }

        ~TelegramService()
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            token = settings.TelegramToken;
            whiteList = settings.TelegramWhiteList;
            chatId = settings.ChatId;
        }

        private void SaveSettings()
        {
            settings.ChatId = chatId.Identifier;
        }

        private async void doStartup()
        {
            mainFormView.Text += $" - ({(await client.GetMeAsync()).Username})";

            if (chatId != null && chatId.Identifier > 0)
            {
                await client.SendTextMessageAsync
                (
                    chatId,
                    "*Telebot*: I'm Up.",
                    parseMode: ParseMode.Markdown
                );
            }
        }

        private void CommandCompleted(object sender, CommandResult e)
        {
            switch (e.SendType)
            {
                case SendType.Text:
                    client.SendTextMessageAsync(e.Message.Chat.Id,
                        e.Text.TrimEnd(),
                        parseMode: ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                    break;
                case SendType.Photo:
                    client.SendPhotoAsync(e.Message.Chat.Id, e.Stream,
                        caption: $"{DateTime.Now.ToString()} by *Telebot*",
                        parseMode: ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                    break;
            }
        }

        private void OnHighTemperature(HighTemperatureMessage obj)
        {
            client.SendTextMessageAsync(chatId, obj.Message, parseMode: ParseMode.Markdown);
        }

        private void BotMessageHandler(object sender, MessageEventArgs e)
        {
            void sendText(string text)
            {
                client.SendTextMessageAsync
                (
                    e.Message.Chat.Id,
                    text,
                    parseMode: ParseMode.Markdown,
                    replyToMessageId: e.Message.MessageId
                );
            }

            void ShowBalloonTip(string text)
            {
                mainFormView.NotifyIcon.ShowBalloonTip
               (
                    1000, 
                    mainFormView.Text, 
                    text, 
                    ToolTipIcon.Info
               );
            }

            if (!whiteList.Exists(x => x.Equals(e.Message.From.Id)))
            {
                sendText("Unauthorized.");
                return;
            }

            if (chatId == null || chatId.Identifier == 0)
            {
                chatId = e.Message.Chat.Id;
            }

            string cmdKey = e.Message.Text;

            if (string.IsNullOrEmpty(cmdKey))
            {
                return;
            }

            if (commands.ContainsKey(cmdKey))
            {
                var cmdInfo = new CommandInfo
                {
                    Message = e.Message,
                    Commands = commands.Values.ToArray()
                };

                commands[cmdKey].Execute(cmdInfo);
            }
            else
            {
                sendText("Undefined command. For commands list, type */help*.");
            }

            string info = $"Received {e.Message.Text} from {e.Message.From.Username}.";

            var item = new LvItem
            {
                DateTime = DateTime.Now.ToString(),
                Text = info
            };

            mainFormView.UpdateListView(item);

            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                ShowBalloonTip(info);
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
