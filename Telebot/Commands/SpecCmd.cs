using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SpecCmd : CommandBase
    {
        public SpecCmd()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var si = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = ".\\SpecInfo.exe"
            };

            Process.Start(si).WaitForExit();

            string filePath = @".\spec.txt";

            var fileStream = new FileStream(filePath, FileMode.Open);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Document,
                Stream = fileStream
            };

            await callback(cmdResult);
        }
    }
}
