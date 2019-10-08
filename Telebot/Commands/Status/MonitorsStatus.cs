using System.Text;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class MonitorsStatus : IStatus
    {
        private readonly IJob<TempChangedArgs>[] _jobs;

        public MonitorsStatus()
        {
            _jobs = Program.tempMonFactory.GetAllEntities();
        }

        public string Execute()
        {
            var result = new StringBuilder();

            foreach (IJob<TempChangedArgs> job in _jobs)
            {
                string name = job.GetType().Name.Replace("TempMon", "");
                string active = job.IsActive.AsReadable();
                result.AppendLine($"*{name}* 🌡️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
