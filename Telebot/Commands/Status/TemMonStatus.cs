using System.Text;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.Temperature;

namespace Telebot.Commands.Status
{
    public class TemMonStatus : IStatus
    {
        public string GetStatus()
        {
            var result = new StringBuilder();

            var _jobs = Program.TempFactory.GetAllEntities();

            foreach (IJob<TempArgs> job in _jobs)
            {
                string name = job.GetType().Name.Replace("TempMon", "");
                string active = job.IsActive.AsReadable();
                result.AppendLine($"*{name}* 🌡️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
