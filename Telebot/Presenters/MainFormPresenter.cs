using BrightIdeasSoftware;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.ScreenCapture;
using Telebot.Settings;
using Telebot.Temperature;
using Telebot.Views;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.Presenters
{
    public class MainFormPresenter : IProfile
    {
        private readonly IMainFormView mainView;
        private readonly ITelebotClient telebotClient;

        public MainFormPresenter(
            IMainFormView mainView,
            ITelebotClient telebotClient,
            IJob<ScreenCaptureArgs>[] screenCaps,
            IJob<TempChangedArgs>[] tempMonitors
        )
        {
            this.mainView = mainView;
            this.mainView.Load += viewLoad;
            this.mainView.FormClosed += viewClosed;
            this.mainView.Resize += viewResize;
            this.mainView.TrayIcon.MouseClick += TrayMouseClick;

            this.telebotClient = telebotClient;
            this.telebotClient.MessageArrived += BotRequestArrival;

            Program.Settings.Handler.AddProfile(this);

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

        private void BotRequestArrival(object sender, MessageArrivedArgs e)
        {
            string date = DateTime.Now.ToString();

            var lvObject = new
            {
                DateTime = date,
                Text = e.MessageText
            };

            mainView.ListView.AddObject(lvObject);

            if (mainView.WindowState == FormWindowState.Minimized)
            {
                mainView.TrayIcon.ShowBalloonTip(
                    1000, mainView.Text, e.MessageText, ToolTipIcon.Info
                );
            }
        }

        private void TrayMouseClick(object sender, MouseEventArgs e)
        {
            mainView.Show();
            mainView.WindowState = FormWindowState.Normal;
            mainView.TrayIcon.Visible = false;
        }

        private void viewResize(object sender, EventArgs e)
        {
            if (mainView.WindowState == FormWindowState.Minimized)
            {
                mainView.Hide();
                mainView.TrayIcon.Visible = true;
            }
        }

        private void viewClosed(object sender, FormClosedEventArgs e)
        {
            if (telebotClient.IsReceiving)
                telebotClient.StopReceiving();
        }

        private void viewLoad(object sender, EventArgs e)
        {
            // Delay job to reduce startup time
            JobManager.AddJob(
                async () => {
                    telebotClient.StartReceiving();
                    await AddBotNameTitle();
                    await SendClientHello();
                }, (s) => s.ToRunOnceIn(3).Seconds()
            );

            if (!Program.isFirstRun)
            {
                RestoreGuiSettings();
            }
        }

        private async Task SendClientHello()
        {
            await telebotClient.SendTextMessageAsync(
                telebotClient.AdminId, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown
            );
        }

        private async Task AddBotNameTitle()
        {
            var me = await telebotClient.GetMeAsync();
            mainView.ListView.Invoke((MethodInvoker)delegate
            {
                mainView.Text += $" - {me.Username}";
            });
        }

        public void SaveChanges()
        {
            Program.Settings.MainView.SaveFormBounds(mainView.Bounds);

            var widths = new List<int>();

            foreach (OLVColumn column in mainView.ListView.AllColumns)
            {
                widths.Add(column.Width);
            }

            Program.Settings.MainView.SaveListView1ColumnsWidth(widths);
        }

        private void RestoreGuiSettings()
        {
            var bounds = Program.Settings.MainView.GetFormBounds();

            if (bounds != null)
                mainView.Bounds = bounds;

            var widths = Program.Settings.MainView.GetListView1ColumnsWidth();

            if (widths != null && widths.Count > 0)
            {
                for (int i = 0; i < widths.Count; i++)
                {
                    mainView.ListView.Columns[i].Width = widths[i];
                }
            }
        }

        private async void ScreenCaptureScheduleHandler(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Capture.ToStream(), "captime.jpg");

            await telebotClient.SendDocumentAsync(
                telebotClient.AdminId, document, thumb: document as InputMedia
            );
        }

        private async void TempMonWarningHandler(object sender, TempChangedArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await telebotClient.SendTextMessageAsync(
                telebotClient.AdminId, text, ParseMode.Markdown
            );
        }

        private StringBuilder text = new StringBuilder();

        private async void TempMonScheduleHandler(object sender, TempChangedArgs e)
        {
            switch (e)
            {
                case null:
                    text.AppendLine("\nFrom *Telebot*");
                    await telebotClient.SendTextMessageAsync(
                        telebotClient.AdminId, text.ToString(), ParseMode.Markdown
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
