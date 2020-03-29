using AutoUpdaterDotNET;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Capture;
using Telebot.Clients;
using Telebot.Extensions;
using Telebot.Intranet;
using Telebot.Jobs;
using Telebot.Models;
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
            IEnumerable<IInetBase> inetJobs,
            IEnumerable<IJob<CaptureArgs>> captureJobs,
            IEnumerable<IJob<TempArgs>> temperatureJobs
        )
        {
            this.view = view;
            this.view.Load += viewLoad;
            this.view.FormClosed += viewClosed;

            this.client = client;
            this.client.Notification += OnNotification;

            foreach (IInetBase inetJob in inetJobs)
            {
                switch (inetJob.Jobtype)
                {
                    case Common.JobType.Monitor:
                        var monitorJob = ((IINetMonitor)inetJob);
                        monitorJob.Disconnected += Disconnected;
                        monitorJob.Connected += Connected;
                        break;
                    case Common.JobType.Scanner:
                        var scannerJob = ((IInetScanner)inetJob);
                        scannerJob.Discovered += Discovered;
                        break;
                }
            }

            foreach (IJob<CaptureArgs> captureJob in captureJobs)
            {
                var Update = captureJob.GetType().GetHandler<CaptureArgs>(this);
                captureJob.Update += Update;
            }

            foreach (IJob<TempArgs> temperatureJob in temperatureJobs)
            {
                var Update = temperatureJob.GetType().GetHandler<TempArgs>(this);
                temperatureJob.Update += Update;
            }

            MessageHub.MessageHub.Instance.Subscribe<Feedback>(OnFeedback);

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
            JobManager.AddJob(async () => {
                    client.Connect();
                    await AddBotNameTitle();
                    await SendClientHello();
                }, (s) => s.ToRunOnceIn(3).Seconds()
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

        private async void OnFeedback(Feedback e)
        {
            await client.SendText(e.Text);
        }

        private async void CaptureScheduleHandler(object sender, CaptureArgs e)
        {
            await client.SendPic(e.Capture.ToMemStream());
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
    }
}
