﻿using FluentScheduler;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class RestartCommand : ICommand
    {
        public RestartCommand()
        {
            Pattern = "/restart";
            Description = "Restart Telebot.";
            OSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var result = new Response("Telebot is restarting...");

            await resp(result);

            JobManager.AddJob(() =>
            {
                Application.Restart();
            }, (s) => s.ToRunOnceIn(2).Seconds()
            );
        }
    }
}
