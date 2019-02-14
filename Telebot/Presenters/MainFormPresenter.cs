using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telebot.Events;
using Telebot.Services;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainFormView mainFormView;
        private readonly ICommunicationService communicationService;

        public MainFormPresenter(IMainFormView mainFormView, ICommunicationService communicationService)
        {
            this.mainFormView = mainFormView;
            this.mainFormView.Load += mainFormView_Load;
            this.mainFormView.FormClosed += mainFormView_FormClosed;
            this.mainFormView.Resize += mainFormView_Resize;
            this.mainFormView.TrayMouseClick += NotifyIcon_MouseClick;

            this.communicationService = communicationService;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            EventAggregator.Instance.Publish(new OnNotifyIconVisibilityArgs(false));
        }

        private void mainFormView_Resize(object sender, EventArgs e)
        {
            if (mainFormView.WindowState == FormWindowState.Minimized)
            {
                mainFormView.Hide();
                EventAggregator.Instance.Publish(new OnNotifyIconVisibilityArgs(true));
            }
        }

        private void mainFormView_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
            communicationService.Stop();
        }

        private void mainFormView_Load(object sender, EventArgs e)
        {
            LoadSettings();
            communicationService.Start();
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
