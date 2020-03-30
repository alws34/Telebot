using Common.Extensions;
using Contracts;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class CapsStatus : IStatus
    {
        private readonly IEnumerable<IPlugin> plugins;

        public CapsStatus(IEnumerable<IPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public string GetStatus()
        {
            var text = new StringBuilder();

            foreach (IPlugin plugin in plugins)
            {
                string name = plugin.GetJobName();
                string active = plugin.GetJobActive().ToReadable();
                text.AppendLine($"*{name}*: {active}");
            }

            return text.ToString().TrimEnd();
        }
    }
}
