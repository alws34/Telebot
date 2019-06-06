using BrightIdeasSoftware;
using System.Windows.Forms;
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
    }
}
