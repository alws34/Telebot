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

        private readonly string token;
        private ChatId chatId;
        private List<int> whiteList;

        public readonly Dictionary<string, ICommand> commands;
        public readonly ITemperatureMonitor tempMonitor;

        private readonly ISettings appSettings;
        private readonly TelegramBotClient botClient;

        public Form1()
        {
            InitializeComponent();

            whiteList = new List<int>();

            listView1.DoubleBuffered(true);

            this.commands = new Dictionary<string, ICommand>();

            var commands = Program.container.GetAllInstances<ICommand>();

            foreach (ICommand command in commands)
            {
                this.commands.Add(command.Name, command);
                command.Completed += Command_Completed;
            }

            tempMonitor = Program.container.GetInstance<ITemperatureMonitor>();
            tempMonitor.TemperatureChanged += TemperatureChanged;

            appSettings = Program.container.GetInstance<ISettings>();

            token = appSettings.GetTelegramToken();

            if (!string.IsNullOrEmpty(token))
            {
                botClient = new TelegramBotClient(token);
                botClient.OnMessage += _botClient_OnMessage;
            }
            else
            {
                MessageBox.Show("No token found for Telegram bot.");
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

        private void _botClient_OnMessage(object sender, MessageEventArgs e)
        {
            void sendText(string text)
            {
                botClient.SendTextMessageAsync(e.Message.Chat.Id, text,
                    parseMode: ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
            }

            if (!whiteList.Exists(x => x.Equals(e.Message.From.Id)))
            {
                sendText("Unauthorized.");
                return;
            }

            string cmdKey = e.Message.Text;

            if (commands.ContainsKey(cmdKey))
            {
                var cmdInfo = new CommandInfo
                {
                    Message = e.Message,
                    Form1 = this
                };

                commands[cmdKey].Execute(cmdInfo);
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
            if (botClient.IsReceiving)
            {
                botClient.StopReceiving();
            }

            SaveSettings();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!string.IsNullOrEmpty(token))
            {
                botClient.StartReceiving();
            }

            if ((chatId != null) && (chatId.Identifier > 0))
            {
                tempMonitor.Start();
            }

            Text += $" - ({(await botClient.GetMeAsync()).Username})";
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
