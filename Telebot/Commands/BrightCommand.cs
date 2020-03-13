﻿using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class BrightCommand : ICommand
    {
        private readonly SystemImpl systemApi;

        public BrightCommand()
        {
            Pattern = "/bright (\\d{1,3})";
            Description = "Adjust workstation's brightness.";

            systemApi = new SystemImpl();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int brvalue = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted brightness to {brvalue}%."
            };

            await resp(result);

            systemApi.SetBrightness(brvalue);
        }
    }
}