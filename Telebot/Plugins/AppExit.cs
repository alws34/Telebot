using Common;
using System.Windows.Forms;

namespace Telebot.Plugins
{
    public class AppExit : IAppExit
    {
        public void Exit()
        {
            Application.Exit();
        }
    }
}
