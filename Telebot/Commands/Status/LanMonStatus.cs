using Telebot.Extensions;

namespace Telebot.Commands.Status
{
    public class LanMonStatus : IStatus
    {
        public string GetStatus()
        {
            var job = Program.LanMonitor;

            string name = job.GetType().Name;
            string status = job.IsActive.AsReadable();

            return $"*{name}:* {status}";
        }
    }
}
