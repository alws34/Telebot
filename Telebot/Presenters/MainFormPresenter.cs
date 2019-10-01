using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Extensions;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using Telebot.Views;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using static Telebot.Settings.SettingsFactory;

namespace Telebot.Presenters
{
    public class MainFormPresenter : Settings.IProfile
    {
        private readonly IMainFormView mainFormView;
        private readonly ITelebotClient telebotClient;

        public MainFormPresenter(
            IMainFormView mainFormView,
            ITelebotClient telebotClient,
            IScreenCapture screenCapture,
            params ITempMon[] tempMonitors
        )
        {
            this.mainFormView = mainFormView;
            this.mainFormView.Load += viewLoad;
            this.mainFormView.Shown += viewShown;
            this.mainFormView.FormClosed += viewFormClosed;
            this.mainFormView.Resize += viewResize;
            this.mainFormView.TrayIcon.MouseClick += NotifyIcon_MouseClick;

            this.telebotClient = telebotClient;
            this.telebotClient.RequestArrival += TelebotClient_RequestArrival;

            screenCapture.ScreenCaptured += ScreenCaptured;

            foreach (ITempMon tempMon in tempMonitors)
            {
                if (tempMon is TempMonWarning)
                    tempMon.TemperatureChanged += WarningTemperatureChanged;
                else if (tempMon is TempMonDurated)
                    tempMon.TemperatureChanged += DuratedTemperatureChanged;
            }
        }

        private void TelebotClient_RequestArrival(object sender, RequestArrivalArgs e)
        {
            mainFormView.ListView.AddObject(e.Item);

            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.TrayIcon.ShowBalloonTip(
                    1000, mainFormView.Text, e.Item.Text, ToolTipIcon.Info
                );
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            mainFormView.TrayIcon.Visible = false;
        }

        private void viewResize(object sender, EventArgs e)
        {
            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.Hide();
                mainFormView.TrayIcon.Visible = true;
            }
        }

        private void viewFormClosed(object sender, FormClosedEventArgs e)
        {
            telebotClient.StopReceiving();
        }

        private void viewLoad(object sender, EventArgs e)
        {
            // to reduce load time
            Task.Delay(2000).ContinueWith((o) =>
            {
                SendClientHello();
                telebotClient.StartReceiving();
            });
        }

        private void viewShown(object sender, EventArgs e)
        {
            RestoreGuiSettings();
            AddBotNameTitle();
        }

        private async void SendClientHello()
        {
            await telebotClient.SendTextMessageAsync(
                telebotClient.AdminId, "*Telebot*: I'm Up.", parseMode: ParseMode.Markdown
            );
        }

        private async void AddBotNameTitle()
        {
            var me = await telebotClient.GetMeAsync();
            mainFormView.Text += $" - {me.Username}";
        }

        public void SaveChanges()
        {
            GuiSettings.SaveFormBounds(mainFormView.Bounds);

            var widths = new List<int>();

            foreach (OLVColumn column in mainFormView.ListView.AllColumns)
            {
                widths.Add(column.Width);
            }

            GuiSettings.SaveListView1ColumnsWidth(widths);
        }

        private void RestoreGuiSettings()
        {
            mainFormView.Bounds = GuiSettings.GetFormBounds();

            var widths = GuiSettings.GetListView1ColumnsWidth();

            if (widths.Count > 0)
            {
                for (int i = 0; i < widths.Count; i++)
                {
                    mainFormView.ListView.Columns[i].Width = widths[i];
                }
            }
        }

        private async void ScreenCaptured(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Capture.ToStream(), "captime.jpg");

            await telebotClient.SendDocumentAsync(
                telebotClient.AdminId, document, thumb: document as InputMedia
            );
        }

        private async void WarningTemperatureChanged(object sender, TempChangedArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            await telebotClient.SendTextMessageAsync(
                telebotClient.AdminId, text, ParseMode.Markdown
            );
        }

        private StringBuilder text = new StringBuilder();

        private async void DuratedTemperatureChanged(object sender, TempChangedArgs e)
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
