using Common.Contracts;
using Common.Models;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Help
{
    public class HelpPlugin : IModule
    {
        private IEnumerable<IModule> plugins;

        public HelpPlugin()
        {
            Pattern = "/help";
            Description = "List of available plugins.";
        }

        public override async void Execute(Request req)
        {
            var builder = new StringBuilder();

            foreach (IModule plugin in plugins)
            {
                builder.AppendLine(plugin.ToString());
            }

            var result = new Response(
                builder.ToString(),
                req.MessageId
            );

            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            plugins = data.IocContainer.GetAllInstances<IModule>();
        }
    }
}
