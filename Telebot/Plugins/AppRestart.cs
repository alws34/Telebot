using Common;
using System;
using System.Windows.Forms;

namespace Telebot.Plugins
{
    public class AppRestart : IAppRestart
    {
        public Action Restart()
        {
            return Application.Restart;
        }
    }
}
