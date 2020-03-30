using Contracts;
using Extensions;
using PluginManager;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class CapsStatus : IStatus
    {
        private readonly IEnumerable<IPlugin> plugins;

        public CapsStatus()
        {
            plugins = Plugins.GetInstance().FindAll(x => x.Pattern.StartsWith("/captime"));
        }

        public string GetStatus()
        {
            var result = new StringBuilder();

            foreach (IPlugin plugin in plugins)
            {
                string name = plugin.GetType().Name.Replace("ScreenCapture", "");
                string active = plugin.GetJobActive().ToReadable();
                result.AppendLine($"*{name}* 🖼️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
