using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Telebot.BusinessLogic
{
    public class WindowsLogic
    {
        public string GetForegroundProcesses()
        {
            var result = new StringBuilder();

            var processes = Process.GetProcesses().Where(x => x.MainWindowHandle != IntPtr.Zero);

            foreach (Process process in processes)
            {
                try
                {
                    string name = process.MainModule.FileVersionInfo.ProductName;
                    int pid = process.Id;
                    result.AppendLine($"{name} ({pid})");
                }
                catch
                {

                }
            }

            return result.ToString().TrimEnd();
        }

        public string GetBackgroundProcesses()
        {
            var result = new StringBuilder();

            var processes = Process.GetProcesses().Where(x => x.SessionId != 0);

            foreach (Process process in processes)
            {
                try
                {
                    string name = process.MainModule.FileVersionInfo.ProductName;
                    int pid = process.Id;
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
