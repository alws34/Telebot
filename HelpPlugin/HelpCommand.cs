using Contracts;
using Models;
using PluginManager;
using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace HelpPlugin
{
    [Export(typeof(IPlugin))]
    public class HelpPlugin : IPlugin
    {
        public HelpPlugin()
        {
            Pattern = "/help";
            Description = "List of available commands.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var s = new StringBuilder();

            var plugins = PluginFactory.Instance.GetAllEntities();

            foreach (IPlugin plugin in plugins)
            {
                s.AppendLine(plugin.ToString());
            }

            var result = new Response(s.ToString());

            await resp(result);
        }
    }
}
