using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Windows.Forms;
using Telebot.Models;

namespace Telebot.Views
{
    public interface IMainFormView
    {
        event EventHandler Load;
        event FormClosedEventHandler FormClosed;
        event EventHandler Resize;

        void UpdateListView(LvItem item);
        void Hide();
        void Show();
        ObjectListView ObjectListView { get; }
        NotifyIcon NotifyIcon { get; }
        Rectangle Bounds { get; set; }
        string Text { get; set; }
        FormWindowState WindowState { get; set; }
    }
}
