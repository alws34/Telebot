using Contracts;
using Extensions;
using PluginManager;

namespace StatusPlugin.Statuses
{
    public class LanMonStatus : IStatus
    {
        private readonly IPlugin Plugin;

        public LanMonStatus()
        {
            Plugin = PluginFactory.Instance.FindEntity(x => x.Pattern.StartsWith("/lan"));
        }

        public string GetStatus()
        {
            string name = Plugin.GetType().Name;
            string status = Plugin.GetJobActive().ToReadable();

            return $"*{name}:* {status}";
        }
    }
}
