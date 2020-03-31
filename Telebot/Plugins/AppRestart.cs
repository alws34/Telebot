using Common;
using System.Windows.Forms;

namespace Telebot.Plugins
{
    public class AppRestart : IAppRestart
    {
        public void Restart()
        {
            Application.Restart();
        }
    }
}
