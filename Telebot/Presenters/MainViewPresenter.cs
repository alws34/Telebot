using Common;
using FluentScheduler;
using System;
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
        private readonly IMainView mainView;
        private readonly IBotClient client;

        public MainViewPresenter(
            IMainView mainView,
            IBotClient client,
            IInetScanner inetScan,
            IINetMonitor inetMon,
            IJob<CaptureArgs>[] caps,
            IJob<TempArgs>[] temps
        )
        {
            this.mainView = mainView;
            this.mainView.Load += viewLoad;
            this.mainView.FormClosed += viewClosed;

            this.client = client;
            this.client.Received += ClientReceived;

            inetScan.Discovered += LanDiscovered;
            inetScan.Notify += Notify;

            inetMon.Connected += LanConnected;
            inetMon.Disconnected += LanDisconnected;
            inetMon.Notify += Notify;

            foreach (BaseCapture cap in caps)
            {
                var handler = CreateEventHandler<CaptureArgs>(cap.GetType());
                cap.Update += handler;
                cap.Notify += Notify;
            }

            foreach (BaseTemp temp in temps)
            {
                var handler = CreateEventHandler<TempArgs>(temp.GetType());
                temp.Update += handler;
                temp.Notify += Notify;
            }
        }

        private async void LanDisconnected(object sender, HostsArg e)
        {
            string text = "Disconnected devices..:\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void LanConnected(object sender, HostsArg e)
        {
            string text = "Connected devices..:\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n";
            }

            await client.SendText(text);
        }

        private async void LanDiscovered(object sender, HostsArg e)
        {
            string text = "";

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

        private void ClientReceived(object sender, ReceivedArgs e)
        {
            mainView.TrayIcon.ShowBalloonTip(
               1000, mainView.Text, e.MessageText, ToolTipIcon.Info
           );
        }

        private void viewClosed(object sender, FormClosedEventArgs e)
        {
            if (client.IsConnected)
                client.Disconnect();
        }

        private void viewLoad(object sender, EventArgs e)
        {
            mainView.Hide();
            mainView.TrayIcon.Icon = mainView.Icon;
            mainView.TrayIcon.Visible = true;

            // Delay job to reduce startup time
            JobManager.AddJob(
                async () =>
                {
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

            mainView.Button1.Invoke((MethodInvoker)delegate
            {
                mainView.TrayIcon.Text += $" - {me.Username}";
            });
        }

        private async void ScreenCaptureScheduleHandler(object sender, CaptureArgs e)
        {
            await client.SendPic(e.Capture.ToStream());
        }

        private async void TempMonWarningHandler(object sender, TempArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await client.SendText(text);
        }

        private StringBuilder text = new StringBuilder();

        private async void TempMonScheduleHandler(object sender, TempArgs e)
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

        private EventHandler<T> CreateEventHandler<T>(Type type)
        {
            string className = type.Name;
            string handlerName = $"{className}Handler";
            return Delegate.CreateDelegate(
                typeof(EventHandler<T>),
                this,
                handlerName
            ) as EventHandler<T>;
        }
    }
}
