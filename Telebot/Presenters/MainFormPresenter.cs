using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telebot.Events;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.Services;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        private ITemperatureMonitor tempMonitor;
        private ICommunicationService communicationService;

        private ISettings settings;

        private readonly IMainFormView mainFormView;

        public MainFormPresenter(IMainFormView mainFormView)
        {
            this.mainFormView = mainFormView;
            mainFormView.Load += mainFormView_Load;
            mainFormView.FormClosed += mainFormView_FormClosed;
            mainFormView.Resize += mainFormView_Resize;
            mainFormView.NotifyIcon.MouseClick += NotifyIcon_MouseClick;

            settings = Program.container.GetInstance<ISettings>();
            communicationService = Program.container.GetInstance<ICommunicationService>();

            tempMonitor = Program.container.GetInstance<ITemperatureMonitor>();
            tempMonitor.TemperatureChanged += TemperatureChanged;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            mainFormView.NotifyIcon.Visible = false;
        }

        private void TemperatureChanged(object sender, IHardwareInfo e)
        {
            string text = "";

            switch (e.DeviceClass)
            {
                case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                    if (e.Value >= CPU_TEMPERATURE_WARNING)
                    {
                        text = $"*[WARNING] CPU_TEMPERATURE*: {e.Value}°C\nFrom *Telebot*";
                    }
                    break;
                case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                    if (e.Value >= GPU_TEMPERATURE_WARNING)
                    {
                        text = $"*[WARNING] {e.DeviceName}*: {e.Value}°C\nFrom *Telebot*";
                    }
                    break;
            }

            EventAggregator.Instance.Publish(new HighTemperatureMessage(text));
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
            communicationService.Stop();

            if (tempMonitor.IsActive)
                tempMonitor.Stop();
        }

        private void mainFormView_Load(object sender, EventArgs e)
        {
            LoadSettings();
            communicationService.Start();

            if (settings.MonitorEnabled)
                tempMonitor.Start();
        }

        private void SaveSettings()
        {
            settings.Form1Bounds = mainFormView.Bounds;

            var widths = new List<int>(mainFormView.ObjectListView.Columns.Count);
            foreach (ColumnHeader column in mainFormView.ObjectListView.Columns) {
                widths.Add(column.Width);
            }
            settings.ListView1ColumnsWidth = widths;

            settings.CPUTemperature = CPU_TEMPERATURE_WARNING;
            settings.GPUTemperature = GPU_TEMPERATURE_WARNING;
        }

        private void LoadSettings()
        {
            mainFormView.Bounds = settings.Form1Bounds;
            var w = settings.ListView1ColumnsWidth;
            for (int i = 0; i < w.Count; i++)
            {
                mainFormView.ObjectListView.Columns[i].Width = w[i];
            }

            CPU_TEMPERATURE_WARNING = settings.CPUTemperature;
            GPU_TEMPERATURE_WARNING = settings.GPUTemperature;
        }
    }
}
