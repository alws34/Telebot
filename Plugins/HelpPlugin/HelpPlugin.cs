using Common.Contracts;
using Common.Models;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Help
{
    public class HelpPlugin : IPlugin
    {
        private IEnumerable<IPlugin> plugins;

        public HelpPlugin()
        {
            Pattern = "/help";
            Description = "List of available plugins.";
        }

        public override async void Execute(Request req)
        {
            var builder = new StringBuilder();

            foreach (IPlugin plugin in plugins)
            {
                builder.AppendLine(plugin.ToString());
            }

            var result = new Response(
                builder.ToString(),
                req.MessageId
            );

            await ResultHandler(result);
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            plugins = data.IocContainer.GetAllInstances<IPlugin>();
        }
    }
}
