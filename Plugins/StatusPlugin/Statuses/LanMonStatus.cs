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
            string text = "";

            string name = plugin.GetJobName();
            bool active = plugin.GetJobActive();

            text += $"*{name}*: {active.ToReadable()}\n";

            return text.TrimEnd('\n');
        }
    }
}
