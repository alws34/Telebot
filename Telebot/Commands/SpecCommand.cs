using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SpecCommand : ICommand
    {
        public SpecCommand()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
            OSVersion = new Version(5, 1);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var si = new ProcessStartInfo(".\\SpecInfo.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            Process.Start(si).WaitForExit();

            string filePath = @".\spec.txt";

            var fileStrm = new FileStream(filePath, FileMode.Open);

            var result = new Response(fileStrm);

            await resp(result);
        }
    }
}
