using Common;
using System;
using System.Windows.Forms;

namespace Telebot.Plugins
{
    public class AppExit : IAppExit
    {
        public Action Exit()
        {
            return Application.Exit;
        }
    }
}
