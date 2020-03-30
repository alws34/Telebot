using Contracts;
using System.Collections.Generic;

namespace StatusPlugin.Statuses
{
    public class TempStatus : IStatus
    {
        private readonly IEnumerable<IPlugin> plugins;

        public TempStatus()
        {
            //plugins = Plugins.Instance.FindAll(x => x.Pattern.StartsWith("/temp"));
        }

        public string GetStatus()
        {
            return "";
            //var result = new StringBuilder();

            //foreach (IPlugin plugin in plugins)
            //{
            //    string name = plugin.GetType().Name.Replace("TempMon", "");
            //    string active = plugin.GetJobActive().ToReadable();
            //    result.AppendLine($"*{name}* 🌡️: {active}");
            //}

            //return result.ToString().TrimEnd();
        }
    }
}
