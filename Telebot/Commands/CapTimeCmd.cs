using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.ScheduledOperations;

namespace Telebot.Commands
{
    public class CapTimeCmd : ICommand
    {
        public string Pattern => "/captime -d (\\d+) -i (\\d+)";

        public string Description => "Schedules a screen capture session.";

        public event EventHandler<CommandResult> Completed;

        private readonly IScheduledScreenCapture scheduledScreenCapture;

        public CapTimeCmd()
        {
            scheduledScreenCapture = Program.container.GetInstance<IScheduledScreenCapture>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Parameters.Groups[2].Value);

            scheduledScreenCapture.Start(duration, interval);

            var result = new CommandResult
            {
                Message = parameters.Message,
                SendType = SendType.Text,
                Text = "Successfully scheduled screen capture."
            };

            Completed?.Invoke(this, result);
        }

        public void ExecuteAsync(object parameter)
        {
            Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
