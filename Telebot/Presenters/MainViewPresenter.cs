using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotSdk.Contracts;
using BotSdk.Models;
using Telebot.Clients;
using Telebot.Views;
using Telegram.Bot.Args;
using Updater;

namespace Telebot.Presenters
{
    public class MainViewPresenter
    {
        private readonly IMainView view;
        private readonly IBotClient client;

        private readonly IEnumerable<IModule> modules;

        public MainViewPresenter(IMainView view, IBotClient client)
        {
            this.view = view;
            this.view.Load += viewLoad;
            this.view.FormClosed += viewClosed;

            this.client = client;
            this.client.OnMessage += ClientOnOnMessage;

            modules = Program.IocContainer.GetAllInstances<IModule>();

            IAppUpdate appUpdate = Program.IocContainer.GetInstance<IAppUpdate>();
            appUpdate.HandleCheck += OnCheckUpdate;
        }

        private async void ClientOnOnMessage(object sender, MessageEventArgs e)
        {
            if (!client.IsAuthorized(e.Message.From.Id))
            {
                await client.SendText("Unauthorized.", e.Message.From.Id, e.Message.MessageId);
                return;
            }

            string pattern = e.Message.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                await client.SendText("Unrecognized pattern.", replyId: e.Message.MessageId);
                return;
            }

            string text = $"Received {pattern} from {e.Message.From.Username}.";

            view.TrayIcon.ShowBalloonTip(
                1000, view.Text, text, ToolTipIcon.Info
            );

            foreach (IModule module in modules)
            {
                Match match = Regex.Match(pattern, $"^{module.Pattern}$");

                if (match.Success)
                {
                    var req = new Request
                    {
                        Groups = match.Groups,
                        MessageId = e.Message.MessageId
                    };

                    module.Execute(req);
                    return;
                }
            }

            await client.SendText("Undefined command. For commands list, type */help*.", replyId: e.Message.MessageId);
        }

        private void viewClosed(object sender, FormClosedEventArgs e)
        {
            if (client.IsConnected)
                client.Disconnect();
        }

        private void viewLoad(object sender, EventArgs e)
        {
            view.Hide();
            view.TrayIcon.Icon = view.Icon;
            view.TrayIcon.Visible = true;

            // Delay task to reduce startup time
            Task.Delay(2500).ContinueWith(async (t) =>
            {
                client.Connect();
                await AddBotNameTitle();
                await SendClientHello();
            });
        }

        private async Task AddBotNameTitle()
        {
            var me = await client.GetMeAsync();

            view.Button1.Invoke((MethodInvoker)delegate
            {
                view.TrayIcon.Text += $" - {me.Username}";
            });
        }

        private async Task SendClientHello()
        {
            await client.SendText("*Telebot*: I'm Up.");
        }

        private async void OnCheckUpdate(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                string text = "";

                switch (args.IsUpdateAvailable)
                {
                    case true:
                        text += "A new version of Telebot is available!\n";
                        text += "run /update dl to update.";
                        await client.SendText(text);
                        break;
                        //case false:
                        //    text += "You are running the latest version of Telebot.";
                        //    await client.SendText(text);
                        //    break;
                }
            }
        }
    }
}
