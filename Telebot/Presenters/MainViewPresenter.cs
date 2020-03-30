using AutoUpdaterDotNET;
using Common.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainViewPresenter
    {
        private readonly IMainView view;
        private readonly IBotClient client;

        public MainViewPresenter(IMainView view, IBotClient client)
        {
            this.view = view;
            this.view.Load += viewLoad;
            this.view.FormClosed += viewClosed;

            this.client = client;
            this.client.Notification += OnNotification;

            //MessageHub.MessageHub.Instance.Subscribe<Feedback>(OnFeedback);

            AutoUpdater.CheckForUpdateEvent += OnCheckUpdate;
        }

        private void OnNotification(object sender, NotificationArgs e)
        {
            view.TrayIcon.ShowBalloonTip(
               1000, view.Text, e.NotificationText, ToolTipIcon.Info
           );
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

            // Delay job to reduce startup time
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

        private async void OnFeedback(Feedback e)
        {
            await client.SendText(e.Text);
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
