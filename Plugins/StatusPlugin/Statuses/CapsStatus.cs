using Common.Extensions;
using Contracts;
using System.Collections.Generic;

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
            string text = "";

            foreach (IPlugin plugin in plugins)
            {
                string name = plugin.GetJobName();
                bool active = plugin.GetJobActive();

                text += $"*{name}*: {active.ToReadable()}\n";
            }

            return text.TrimEnd('\n');
        }
    }
}
