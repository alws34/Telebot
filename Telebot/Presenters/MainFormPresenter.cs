using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using Telebot.Views;
using Telegram.Bot.Types.Enums;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainFormView mainFormView;
        private readonly ITelebotClient telebotClient;

        public MainFormPresenter(
            IMainFormView mainFormView,
            ITelebotClient telebotClient,
            IScreenCapture screenCapture,
            ITemperatureMonitor[] temperatureMonitors
            )
        {
            this.mainFormView = mainFormView;
            this.mainFormView.Load += mainFormView_Load;
            this.mainFormView.FormClosed += mainFormView_FormClosed;
            this.mainFormView.Resize += mainFormView_Resize;
            this.mainFormView.NotifyIcon.MouseClick += NotifyIcon_MouseClick;

            this.telebotClient = telebotClient;
            this.telebotClient.RequestArrival += TelebotClient_RequestArrival;

            var screenCaptureAggregator = new ScreenCaptureAggregator(telebotClient, screenCapture);
            var temperatureMonitorAggregator = new TemperatureMonitorAggregator(telebotClient, temperatureMonitors);
        }

        private void TelebotClient_RequestArrival(object sender, RequestArrivalArgs e)
        {
            mainFormView.ObjectListView.AddObject(e.Item);

            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.NotifyIcon.ShowBalloonTip(
                    1000, mainFormView.Text, e.Item.Text, ToolTipIcon.Info);
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            mainFormView.NotifyIcon.Visible = false;
        }

        private void mainFormView_Resize(object sender, EventArgs e)
        {
            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.Hide();
                mainFormView.NotifyIcon.Visible = true;
            }
        }

        private void mainFormView_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
            telebotClient.StopReceiving();
        }

        private void mainFormView_Load(object sender, EventArgs e)
        {
            LoadSettings();
            SetTitleUsername();
            SendClientHello();
            telebotClient.StartReceiving();
        }

        private async void SendClientHello()
        {
            await telebotClient.SendTextMessageAsync
            (
                telebotClient.AdminID,
                "*Telebot*: I'm Up.",
                parseMode: ParseMode.Markdown
            );
        }

        private async void SetTitleUsername()
        {
            var me = await telebotClient.GetMeAsync();
            mainFormView.Text += $" ({me.Username})";
        }

        private void SaveSettings()
        {
            Program.appSettings.Form1Bounds = mainFormView.Bounds;

            var widths = new List<int>(mainFormView.ObjectListView.Columns.Count);
            foreach (ColumnHeader column in mainFormView.ObjectListView.Columns)
            {
                widths.Add(column.Width);
            }
            Program.appSettings.ListView1ColumnsWidth = widths;
        }

        private void LoadSettings()
        {
            mainFormView.Bounds = Program.appSettings.Form1Bounds;
            var w = Program.appSettings.ListView1ColumnsWidth;
            for (int i = 0; i < w.Count; i++)
            {
                mainFormView.ObjectListView.Columns[i].Width = w[i];
            }
        }
    }
}
