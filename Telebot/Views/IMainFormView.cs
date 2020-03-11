using System;
using System.Drawing;
using System.Windows.Forms;

namespace Telebot.Views
{
    public interface IMainFormView
    {
        event EventHandler Load;
        event FormClosedEventHandler FormClosed;

        void Hide();

        string Text { get; set; }
        Button Button1 { get; }
        NotifyIcon TrayIcon { get; }
        Icon Icon { get; }
    }
}
