using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telebot.Contracts;
using Telebot.Models;
using Telebot.Views;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        private string token;
        private ChatId chatId;
        private List<int> whiteList;
        private ITemperatureMonitor tempMonitor;

        private Dictionary<string, ICommand> commands { get; }

        private ISettings appSettings;
        private TelegramBotClient botClient;

        private readonly IMainFormView mainFormView;

        public MainFormPresenter(IMainFormView mainFormView)
        {
            whiteList = new List<int>();
            this.commands = new Dictionary<string, ICommand>();

            this.mainFormView = mainFormView;
            mainFormView.Load += mainFormView_Load;
            mainFormView.FormClosed += mainFormView_FormClosed;
            mainFormView.Resize += mainFormView_Resize;
            mainFormView.NotifyIcon.MouseClick += NotifyIcon_MouseClick;

            appSettings = Program.container.GetInstance<ISettings>();

            tempMonitor = Program.container.GetInstance<ITemperatureMonitor>();
            tempMonitor.TemperatureChanged += TempMonitor_TemperatureChanged;

            var commands = Program.container.GetAllInstances<ICommand>();
            foreach (ICommand command in commands)
            {
                this.commands.Add(command.Name, command);
                command.Completed += Command_Completed;
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            mainFormView.NotifyIcon.Visible = false;
        }

        private void TempMonitor_TemperatureChanged(object sender, IHardwareInfo e)
        {
            string text = "";

            switch (e.DeviceClass)
            {
                case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                    if (e.Value >= CPU_TEMPERATURE_WARNING)
                    {
                        text = $"*[WARNING] CPU_TEMPERATURE*: {e.Value}°C\nFrom *Telebot*";
                    }
                    break;
                case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                    if (e.Value >= GPU_TEMPERATURE_WARNING)
                    {
                        text = $"*[WARNING] {e.DeviceName}*: {e.Value}°C\nFrom *Telebot*";
                    }
                    break;
            }

            botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Markdown);
        }

        private void mainFormView_Resize(object sender, EventArgs e)
        {
            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.Hide();
                mainFormView.NotifyIcon.Visible = true;
            }
        }

        private void Command_Completed(object sender, CommandResult e)
        {
            switch (e.SendType)
            {
                case SendType.Text:
                    botClient.SendTextMessageAsync(e.Message.Chat.Id,
                        e.Text.TrimEnd(),
                        parseMode: ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                    break;
                case SendType.Photo:
                    botClient.SendPhotoAsync(e.Message.Chat.Id, e.Stream,
                        caption: $"{DateTime.Now.ToString()} by *Telebot*",
                        parseMode: ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                    break;
            }
        }

        private void mainFormView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (botClient != null && botClient.IsReceiving) {
                botClient.StopReceiving();
            }

            SaveSettings();
        }

        private async void mainFormView_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!string.IsNullOrEmpty(token))
            {
                botClient = new TelegramBotClient(token);
                botClient.OnMessage += BotClient_OnMessage;
            }

            if (botClient != null)
            {
                botClient.StartReceiving();
                mainFormView.Text += $" - ({(await botClient.GetMeAsync()).Username})";

                if ((chatId != null) && (chatId.Identifier > 0))
                {
                    if (appSettings.MonitorEnabled)
                    {
                        tempMonitor.Start();
                    }

                    await botClient.SendTextMessageAsync
                    (
                        chatId,
                        "*Telebot*: I'm Up.",
                        parseMode: ParseMode.Markdown
                    );
                }
            }
        }

        private void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            void sendText(string text)
            {
                botClient.SendTextMessageAsync
                (
                    e.Message.Chat.Id,
                    text,
                    parseMode: ParseMode.Markdown,
                    replyToMessageId: e.Message.MessageId
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

            if (string.IsNullOrEmpty(cmdKey)) {
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

        private void ShowBalloonTip(string text)
        {
            mainFormView.NotifyIcon.ShowBalloonTip(1000, mainFormView.Text, text, ToolTipIcon.Info);
        }

        private void SaveSettings()
        {
            appSettings.Form1Bounds = mainFormView.Bounds;

            var widths = new List<int>(mainFormView.ObjectListView.Columns.Count);
            foreach (ColumnHeader column in mainFormView.ObjectListView.Columns) {
                widths.Add(column.Width);
            }
            appSettings.ListView1ColumnsWidth = widths;

            appSettings.ChatId = chatId.Identifier;

            appSettings.CPUTemperature = CPU_TEMPERATURE_WARNING;
            appSettings.GPUTemperature = GPU_TEMPERATURE_WARNING;
        }

        private void LoadSettings()
        {
            //GUI Settings
            mainFormView.Bounds = appSettings.Form1Bounds;
            var w = appSettings.ListView1ColumnsWidth;
            for (int i = 0; i < w.Count; i++)
            {
                mainFormView.ObjectListView.Columns[i].Width = w[i];
            }

            //Telegram Settings
            token = appSettings.TelegramToken;
            whiteList = appSettings.TelegramWhiteList;
            chatId = appSettings.ChatId;

            //Temperature Monitor Settings
            CPU_TEMPERATURE_WARNING = appSettings.CPUTemperature;
            GPU_TEMPERATURE_WARNING = appSettings.GPUTemperature;
        }
    }
}
