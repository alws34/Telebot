using Contracts;
using Extensions;
using PluginManager;
using System.Collections.Generic;
using System.Text;

namespace StatusPlugin.Statuses
{
    public class TempStatus : IStatus
    {
        private readonly IEnumerable<IPlugin> Plugins;

        public TempStatus()
        {
            Plugins = PluginFactory.Instance.FindAll(x => x.Pattern.StartsWith("/temp"));
        }

        public string GetStatus()
        {
            var result = new StringBuilder();

            var plugins = PluginFactory.Instance.FindEntity(x => x.Pattern.StartsWith("/temp"));

            foreach (IPlugin Plugin in Plugins)
            {
                string name = Plugin.GetType().Name.Replace("TempMon", "");
                string active = Plugin.GetJobActive().ToReadable();
                result.AppendLine($"*{name}* 🌡️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
