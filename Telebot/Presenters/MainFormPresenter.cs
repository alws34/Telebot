using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telebot.Events;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.Monitors.Factories;
using Telebot.ScheduledOperations;
using Telebot.Services;
using Telebot.Views;

namespace Telebot.Presenters
{
    public class MainFormPresenter
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        private readonly IEnumerable<ITemperatureMonitor> temperatureMonitors;
        private readonly IScheduledScreenCapture scheduledScreenCapture;
        private readonly ICommunicationService communicationService;

        private ISettings settings;

        private readonly IMainFormView mainFormView;

        public MainFormPresenter(IMainFormView mainFormView)
        {
            this.mainFormView = mainFormView;
            this.mainFormView.Load += mainFormView_Load;
            this.mainFormView.FormClosed += mainFormView_FormClosed;
            this.mainFormView.Resize += mainFormView_Resize;
            this.mainFormView.TrayMouseClick += NotifyIcon_MouseClick;

            settings = Program.container.GetInstance<ISettings>();
            communicationService = Program.container.GetInstance<ICommunicationService>();

            temperatureMonitors = TempMonitorFactory.Instance.GetAllTemperatureMonitors();
            foreach (ITemperatureMonitor temperatureMonitor in temperatureMonitors)
            {
                //TemperatureChanged
                temperatureMonitor.TemperatureChanged += (s, o) => { Console.WriteLine("Hey"); };
            }

            scheduledScreenCapture = Program.container.GetInstance<IScheduledScreenCapture>();
            scheduledScreenCapture.Captured += ScheduledScreenCaptureCaptured;
        }

        private void ScheduledScreenCaptureCaptured(object sender, Bitmap e)
        {
            EventAggregator.Instance.Publish(new OnScreenCaptureArgs(e));
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            mainFormView.Show();
            mainFormView.WindowState = FormWindowState.Normal;
            EventAggregator.Instance.Publish(new OnNotifyIconVisibilityArgs(false));
        }

        private void TemperatureChanged(object sender, IEnumerable<HardwareInfo> devices)
        {
            if (sender is PermanentTempMonitor)
            {
                foreach (HardwareInfo device in devices)
                {
                    if (device.Value >= CPU_TEMPERATURE_WARNING || device.Value >= GPU_TEMPERATURE_WARNING)
                    {
                        string text = $"*[WARNING] {device.DeviceName}*: {device.Value}°C\nFrom *Telebot*";
                        EventAggregator.Instance.Publish(new OnHighTemperatureArgs(text));
                    }
                }
            }
            else if (sender is ScheduledTempMonitor)
            {
                var text = new StringBuilder();

                foreach (HardwareInfo device in devices)
                {
                    text.AppendLine($"*{device.DeviceName}*: {device.Value}°C");
                }

                text.AppendLine("\nFrom *Telebot*");

                EventAggregator.Instance.Publish(new OnHighTemperatureArgs(text.ToString()));
            }
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
            settings.Form1Bounds = mainFormView.Bounds;

            var widths = new List<int>(mainFormView.ObjectListView.Columns.Count);
            foreach (ColumnHeader column in mainFormView.ObjectListView.Columns)
            {
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
