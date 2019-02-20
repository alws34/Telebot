using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Telebot.BusinessLogic
{
    public class WindowsLogic
    {
        public string GetActiveApplications()
        {
            var result = new StringBuilder();

            var apps = Process.GetProcesses().Where(x => x.MainWindowHandle != IntPtr.Zero);

            foreach (Process app in apps)
            {
                try
                {
                    string name = app.MainModule.FileVersionInfo.ProductName;
                    int pid = app.Id;
                    result.AppendLine($"{name} ({pid})");
                }
                catch
                {

                }
            }

            return result.ToString().TrimEnd();
        }
    }
}
