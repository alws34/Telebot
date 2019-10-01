using BrightIdeasSoftware;
using System.Windows.Forms;
using Telebot.Views;

namespace Telebot
{
    public partial class MainForm : Form, IMainFormView
    {
        public ObjectListView ListView => fastOlv;
        public NotifyIcon TrayIcon => notifyIcon1;

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
