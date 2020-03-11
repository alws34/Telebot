using FluentScheduler;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using Telebot.Views;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainFormView mainView;
        private readonly ITelebotClient client;

        public MainFormPresenter(
            IMainFormView mainView,
            ITelebotClient client,
            IJob<ScreenCaptureArgs>[] screenCaps,
            IJob<TempChangedArgs>[] tempMonitors
        )
        {
            this.mainView = mainView;
            this.mainView.Load += viewLoad;
            this.mainView.FormClosed += viewClosed;

            this.client = client;
            this.client.MessageArrived += BotMessageArrived;

            foreach (IJob<ScreenCaptureArgs> screenCap in screenCaps)
            {
                var handler = CreateEventHandler<ScreenCaptureArgs>(screenCap.GetType());
                screenCap.Update += handler;
            }

            foreach (IJob<TempChangedArgs> tempMon in tempMonitors)
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

        private void BotMessageArrived(object sender, MessageArrivedArgs e)
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
            await client.SendTextMessageAsync(
                client.AdminId, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown
            );
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
            var document = new InputOnlineFile(e.Capture.ToStream(), "captime.jpg");

            await client.SendDocumentAsync(
                client.AdminId, document, thumb: document as InputMedia
            );
        }

        private async void TempMonWarningHandler(object sender, TempChangedArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await client.SendTextMessageAsync(
                client.AdminId, text, ParseMode.Markdown
            );
        }

        private StringBuilder text = new StringBuilder();

        private async void TempMonScheduleHandler(object sender, TempChangedArgs e)
        {
            switch (e)
            {
                case null:
                    text.AppendLine("\nFrom *Telebot*");
                    await client.SendTextMessageAsync(
                        client.AdminId, text.ToString(), ParseMode.Markdown
                    );
                    text.Clear();
                    break;
                default:
                    text.AppendLine($"*{e.DeviceName}*: {e.Temperature}°C");
                    break;
            }
        }
    }
}
