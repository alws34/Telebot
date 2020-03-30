using Common.Extensions;
using Contracts;

namespace StatusPlugin.Statuses
{
    public class LanMonStatus : IStatus
    {
        private readonly IPlugin plugin;

        public LanMonStatus(IPlugin plugin)
        {
            this.plugin = plugin;
        }

        public string GetStatus()
        {
            string name = plugin.GetJobName();
            string status = plugin.GetJobActive().ToReadable();

            return $"*{name}:* {status}";
        }
    }
}
