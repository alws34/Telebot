using AutoUpdaterDotNET;
using Common.Models;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Capture;
using Telebot.Clients;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.Intranet;
using Telebot.Temperature;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainViewPresenter
    {
        private readonly IMainView view;
        private readonly IBotClient client;

        public MainViewPresenter(
            IMainView view,
            IBotClient client,
            IInetScanner inetScan,
            IINetMonitor inetMon,
            IEnumerable<IJob<CaptureArgs>> caps,
            IEnumerable<IJob<TempArgs>> temps
        )
        {
            this.view = view;
            this.view.Load += viewLoad;
            this.view.FormClosed += viewClosed;

            this.client = client;
            this.client.Notification += ClientNotification;

            inetScan.Discovered += Discovered;
            inetScan.Feedback += Notify;

            inetMon.Connected += Connected;
            inetMon.Disconnected += Disconnected;
            inetMon.Feedback += Notify;

            foreach (IJob<CaptureArgs> cap in caps)
            {
                var Update = GetHandler<CaptureArgs>(cap.GetType());
                cap.Update += Update;
                cap.Feedback += Notify;
            }

            foreach (IJob<TempArgs> temp in temps)
            {
                var Update = GetHandler<TempArgs>(temp.GetType());
                temp.Update += Update;
                temp.Feedback += Notify;
            }

            AutoUpdater.AppCastURL = ConfigurationManager.AppSettings["updateUrl"];
            AutoUpdater.CheckForUpdateEvent += OnCheckUpdate;
        }

        private void ClientNotification(object sender, NotificationArgs e)
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
            JobManager.AddJob(
                async () =>
                {
                    client.Connect();
                    await AddBotNameTitle();
                    await SendClientHello();
                }, (s) => s.ToRunOnceIn(3).Seconds()
            );

            JobManager.AddJob(
                () =>
                {
                    AutoUpdater.Start();
                }, (s) => s.WithName("CheckForUpdate").ToRunEvery(1).Hours()
            );
        }

        private async Task SendClientHello()
        {
            await client.SendText("*Telebot*: I'm Up.");
        }

        private async Task AddBotNameTitle()
        {
            var me = await client.GetMeAsync();

            view.Button1.Invoke((MethodInvoker)delegate
            {
                view.TrayIcon.Text += $" - {me.Username}";
            });
        }

        private async void Disconnected(object sender, HostsArg e)
        {
            string text = "Disconnected:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void Connected(object sender, HostsArg e)
        {
            string text = "Connected:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void Discovered(object sender, HostsArg e)
        {
            string text = "Discovered:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n\n";
            }

            await client.SendText(text);
        }

        private async void Notify(object sender, FeedbackArgs e)
        {
            await client.SendText(e.Text);
        }

        private async void CaptureScheduleHandler(object sender, CaptureArgs e)
        {
            await client.SendPic(e.Capture.ToStream());
        }

        private async void TempWarningHandler(object sender, TempArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await client.SendText(text);
        }

        private StringBuilder text = new StringBuilder();

        private async void TempScheduleHandler(object sender, TempArgs e)
        {
            switch (e)
            {
                case null:
                    text.AppendLine("\nFrom *Telebot*");
                    await client.SendText(text.ToString());
                    text.Clear();
                    break;
                default:
                    text.AppendLine($"*{e.DeviceName}*: {e.Temperature}°C");
                    break;
            }
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

        private EventHandler<T> GetHandler<T>(Type type)
        {
            string handlerName = $"{type.Name}Handler";
            return Delegate.CreateDelegate(
                typeof(EventHandler<T>), this, handlerName
            ) as EventHandler<T>;
        }
    }
}
