using FluentScheduler;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.Models;
using Telebot.Network;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainView mainView;
        private readonly IBotClient client;
        private readonly INetMonitor netMon;

        public MainFormPresenter(
            IMainView mainView,
            IBotClient client,
            INetMonitor netMon
        )
        {
            this.mainView = mainView;
            this.mainView.Load += viewLoad;
            this.mainView.FormClosed += viewClosed;

            this.client = client;
            this.client.Received += BotMessageArrived;

            this.netMon = netMon;
            this.netMon.Discovered += NetMon_Discovered;

            var scrnCaps = Program.ScreenFactory.GetAllEntities();

            foreach (IJob<ScreenCaptureArgs> scrnCap in scrnCaps)
            {
                var handler = CreateEventHandler<ScreenCaptureArgs>(scrnCap.GetType());
                scrnCap.Update += handler;
            }

            var tempMons = Program.TempFactory.GetAllEntities();

            foreach (IJob<TempChangedArgs> tempMon in tempMons)
            {
                var handler = CreateEventHandler<TempChangedArgs>(tempMon.GetType());
                tempMon.Update += handler;
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

        private void BotMessageArrived(object sender, ReceivedArgs e)
        {
            mainView.TrayIcon.ShowBalloonTip(
               1000, mainView.Text, e.MessageText, ToolTipIcon.Info
           );
        }

        private void viewClosed(object sender, FormClosedEventArgs e)
        {
            if (client.IsReceiving)
                client.StopReceiving();
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
                    client.StartReceiving();
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

        private async void ScreenCaptureScheduleHandler(object sender, ScreenCaptureArgs e)
        {
            await client.SendPic(e.Capture.ToStream());
        }

        private async void TempMonWarningHandler(object sender, TempChangedArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await client.SendText(text);
        }

        private StringBuilder text = new StringBuilder();

        private async void TempMonScheduleHandler(object sender, TempChangedArgs e)
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

        private async void NetMon_Discovered(object sender, HostsArg e)
        {
            string text = "";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n\n";
            }

            await client.SendText(text);
        }
    }
}
