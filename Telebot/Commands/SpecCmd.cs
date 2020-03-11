using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SpecCmd : BaseCommand
    {
        public SpecCmd()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
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

            var result = new CommandResult
            {
                ResultType = ResultType.Document,
                Raw = fileStream
            };

            await cbResult(result);
        }
    }
}
