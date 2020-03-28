using Telebot.Extensions;
using Telebot.Intranet;

namespace Telebot.Commands.Status
{
    public class LanMonStatus : IStatus
    {
        private readonly IINetMonitor monitor;

        public LanMonStatus()
        {
            monitor = Program.InetFactory.FindEntity(
                x => x.Jobtype == Common.JobType.Monitor
            ) as IINetMonitor;
        }

        public string GetStatus()
        {
            string name = monitor.GetType().Name;
            string status = monitor.IsActive.AsReadable();

            return $"*{name}:* {status}";
        }
    }
}
