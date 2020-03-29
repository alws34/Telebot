using Contracts;
using Extensions;
using PluginManager;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class CapsStatus : IStatus
    {
        private readonly IEnumerable<IPlugin> Plugins;

        public CapsStatus()
        {
            Plugins = PluginFactory.Instance.FindAll(x => x.Pattern.StartsWith("/captime"));
        }

        public string GetStatus()
        {
            var result = new StringBuilder();

            foreach (IPlugin Plugin in Plugins)
            {
                string name = Plugin.GetType().Name.Replace("ScreenCapture", "");
                string active = Plugin.GetJobActive().ToReadable();
                result.AppendLine($"*{name}* 🖼️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
