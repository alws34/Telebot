using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SpecCmd : ICommand
    {
        public SpecCmd()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
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

            var result = new Response
            {
                ResultType = ResultType.Document,
                Raw = fileStream
            };

            await resp(result);
        }
    }
}
