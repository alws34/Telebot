using Contracts;
using Contracts.Factories;
using Models;
using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Help
{
    [Export(typeof(IPlugin))]
    public class HelpPlugin : IPlugin
    {
        private IFactory<IPlugin> Plugins;

        public HelpPlugin()
        {
            Pattern = "/help";
            Description = "List of available plugins.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var builder = new StringBuilder();

            foreach (IPlugin plugin in Plugins.GetAllEntities())
            {
                builder.AppendLine(plugin.ToString());
            }

            var result = new Response(builder.ToString());

            await resp(result);
        }

        public override void Initialize(PluginData data)
        {
            Plugins = data.Plugins;
        }
    }
}
