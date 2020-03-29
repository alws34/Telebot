﻿using System.Text;
using Telebot.Extensions;
using Telebot.Jobs;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class TempStatus : IStatus
    {
        public string GetStatus()
        {
            var result = new StringBuilder();

            var _jobs = Program.TempFactory.GetAllEntities();

            foreach (IJob<TempArgs> job in _jobs)
            {
                string name = job.GetType().Name.Replace("TempMon", "");
                string active = job.Active.ToReadable();
                result.AppendLine($"*{name}* 🌡️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
