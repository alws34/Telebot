﻿using AutoUpdaterDotNET;
using Common;
using FluentScheduler;
using System;
using System.IO;
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
            IJob<CaptureArgs>[] caps,
            IJob<TempArgs>[] temps
        )
        {
            this.view = view;
            this.view.Load += viewLoad;
            this.view.FormClosed += viewClosed;

            this.client = client;
            this.client.Received += ClientReceived;

            inetScan.Discovered += LanDiscovered;
            inetScan.Notify += Notify;

            inetMon.Connected += LanConnected;
            inetMon.Disconnected += LanDisconnected;
            inetMon.Notify += Notify;

            foreach (BaseCapture cap in caps)
            {
                var Update = GetJobUpdate<CaptureArgs>(cap.GetType());
                cap.Update += Update;
                cap.Notify += Notify;
            }

            foreach (BaseTemp temp in temps)
            {
                var Update = GetJobUpdate<TempArgs>(temp.GetType());
                temp.Update += Update;
                temp.Notify += Notify;
            }

            AutoUpdater.AppCastURL = File.ReadAllText(".\\xmlurl.txt");
            AutoUpdater.CheckForUpdateEvent += OnCheckUpdate;
        }

        private void ClientReceived(object sender, ReceivedArgs e)
        {
            view.TrayIcon.ShowBalloonTip(
               1000, view.Text, e.MessageText, ToolTipIcon.Info
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
                async () => {
                    client.Connect();
                    await AddBotNameTitle();
                    await SendClientHello();
                }, (s) => s.ToRunOnceIn(3).Seconds()
            );

            JobManager.AddJob(() => {
                AutoUpdater.Start();
            }, (s) => s.ToRunEvery(1).Hours()
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

        private async void LanDisconnected(object sender, HostsArg e)
        {
            string text = "New device(s) disconnected on the local network:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void LanConnected(object sender, HostsArg e)
        {
            string text = "New device(s) connected on the local network:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void LanDiscovered(object sender, HostsArg e)
        {
            string text = "Connected devices on the local network:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n\n";
            }

            await client.SendText(text);
        }

        private async void Notify(object sender, NotifyArg e)
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
                switch (args.IsUpdateAvailable)
                {
                    case true:
                        string text1 = "";
                        text1 += "A new version of Telebot is available!\n";
                        text1 += "check /update for more info.";

                        await client.SendText(text1);
                        break;
                    case false:
                        string text = "You are running the latest version of Telebot.";
                        await client.SendText(text);
                        break;
                }
            }
        }

        private EventHandler<T> GetJobUpdate<T>(Type type)
        {
            string handlerName = $"{type.Name}Handler";
            return Delegate.CreateDelegate(
                typeof(EventHandler<T>), this, handlerName
            ) as EventHandler<T>;
        }
    }
}
