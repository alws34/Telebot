using Contracts;

namespace StatusPlugin.Statuses
{
    public class LanMonStatus : IStatus
    {
        private readonly IPlugin Plugin;

        public LanMonStatus()
        {
            //Plugin = Plugins.Instance.FindEntity(x => x.Pattern.StartsWith("/lan"));
        }

        public string GetStatus()
        {
            return "";
            //string name = Plugin.GetType().Name;
            //string status = Plugin.GetJobActive().ToReadable();

            //return $"*{name}:* {status}";
        }
    }
}
