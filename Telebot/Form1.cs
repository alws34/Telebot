using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot
{
    public partial class Form1 : Form
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        private string token;
        private ChatId chatId;
        private List<int> whiteList;
        private ITemperatureMonitor tempMonitor;

        public Dictionary<string, ICommand> Commands { get; }

        private ISettings appSettings;
        private TelegramBotClient botClient;

        public Form1()
        {
            InitializeComponent();
            listView1.DoubleBuffered(true);

            whiteList = new List<int>();
            Commands = new Dictionary<string, ICommand>();
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

        private void botClient_OnMessage(object sender, MessageEventArgs e)
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

            if (chatId == null || chatId.Identifier == 0) {
                chatId = e.Message.Chat.Id;
            }

            string cmdKey = e.Message.Text;

            if (string.IsNullOrEmpty(cmdKey)) {
                return;
            }

            if (Commands.ContainsKey(cmdKey))
            {
                var cmdInfo = new CommandInfo
                {
                    Message = e.Message
                };

                Commands[cmdKey].Execute(cmdInfo);
            }
            else
            {
                sendText("Undefined command. For commands list, type */help*.");
            }

            string title = DateTime.Now.ToString();
            string info = $"Received {e.Message.Text} from {e.Message.From.Username}.";

            listView1.Invoke((MethodInvoker)delegate
            {
                listView1.Items.Add(CreateLvItem(title, info));
            });

            if (WindowState == FormWindowState.Minimized)
            {
                ShowBalloonTip(info);
            }
        }

        private void TemperatureChanged(object sender, IHardwareInfo e)
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

        private ListViewItem CreateLvItem(string title, string info)
        {
            var result = new ListViewItem(title);
            result.SubItems.Add(info);
            return result;
        }

        private void ShowBalloonTip(string text)
        {
            notifyIcon1.ShowBalloonTip(1000, Text, text, ToolTipIcon.Info);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (botClient != null && botClient.IsReceiving)
            {
                botClient.StopReceiving();
            }

            SaveSettings();
        }

        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            appSettings = Program.container.GetInstance<ISettings>();

            LoadSettings();

            var commands = Program.container.GetAllInstances<ICommand>();
            foreach (ICommand command in commands)
            {
                Commands.Add(command.Name, command);
                command.Completed += Command_Completed;
            }

            tempMonitor = Program.container.GetInstance<ITemperatureMonitor>();
            tempMonitor.TemperatureChanged += TemperatureChanged;

            token = appSettings.GetTelegramToken();

            if (!string.IsNullOrEmpty(token))
            {
                botClient = new TelegramBotClient(token);
                botClient.OnMessage += botClient_OnMessage;
            }

            if (botClient != null)
            {
                botClient.StartReceiving();
                Text += $" - ({(await botClient.GetMeAsync()).Username})";

                if ((chatId != null) && (chatId.Identifier > 0))
                {
                    if (appSettings.GetMonitorEnabled())
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void SaveSettings()
        {
            appSettings.SetForm1Bounds(Bounds);

            var widths = new List<int>(listView1.Columns.Count);
            foreach (ColumnHeader column in listView1.Columns)
            {
                widths.Add(column.Width);
            }
            appSettings.SetListView1ColumnsWidth(widths);

            appSettings.SetChatId(chatId.Identifier);

            appSettings.SetCPUTemperature(CPU_TEMPERATURE_WARNING);
            appSettings.SetGPUTemperature(GPU_TEMPERATURE_WARNING);
        }

        private void LoadSettings()
        {
            //GUI Settings
            Bounds = appSettings.GetForm1Bounds();
            var w = appSettings.GetListView1ColumnsWidth();
            for (int i = 0; i < w.Count; i++)
            {
                listView1.Columns[i].Width = w[i];
            }

            //Telegram Settings
            whiteList = appSettings.GetTelegramWhiteList();
            chatId = appSettings.GetChatId();

            //Temperature Monitor Settings
            CPU_TEMPERATURE_WARNING = appSettings.GetCPUTemperature();
            GPU_TEMPERATURE_WARNING = appSettings.GetGPUTemperature();
        }
    }
}
