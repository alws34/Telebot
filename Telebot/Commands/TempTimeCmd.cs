using System;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.ScheduledOperations;

namespace Telebot.Commands
{
    public class TempTimeCmd : ICommand
    {
        public string Pattern => "/temptime -d (\\d+) -i (\\d+)";

        public string Description => "Schedules a temperature monitor.";

        public event EventHandler<CommandResult> Completed;

        private readonly IScheduledTemperatureMonitor scheduledTemperatureMonitor;

        public TempTimeCmd()
        {
            scheduledTemperatureMonitor = Program.container.GetInstance<IScheduledTemperatureMonitor>();
        }

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Parameters.Groups[2].Value);

            scheduledTemperatureMonitor.Start(duration, interval);

            var result = new CommandResult
            {
                Message = parameters.Message,
                SendType = SendType.Text,
                Text = "Successfully scheduled temperature monitor."
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
