using System.Windows.Forms;
using Telebot.Views;

namespace Telebot
{
    public partial class MainForm : Form, IMainView
    {
        public Button Button1 => button1; // This is a "hack" to invoke code on ui thread
        public NotifyIcon TrayIcon => notifyIcon1;

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
