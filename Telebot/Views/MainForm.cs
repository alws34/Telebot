using System.Windows.Forms;
using BrightIdeasSoftware;
using Telebot.Models;
using Telebot.Views;

namespace Telebot
{
    public partial class MainForm : Form, IMainFormView
    {
        public ObjectListView ObjectListView => objectListView1;

        public NotifyIcon NotifyIcon => notifyIcon1;

        public MainForm()
        {
            InitializeComponent();
        }

        public void UpdateListView(LvItem item)
        {
            objectListView1.AddObject(item);
        }
    }
}
