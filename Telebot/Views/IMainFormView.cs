using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Telebot.Views
{
    public interface IMainFormView
    {
        event EventHandler Load;
        event EventHandler Shown;
        event FormClosedEventHandler FormClosed;
        event EventHandler Resize;

        string Text { get; set; }
        void Hide();
        void Show();
        ObjectListView ListView { get; }
        NotifyIcon TrayIcon { get; }
        Rectangle Bounds { get; set; }
        FormWindowState WindowState { get; set; }
    }
}
