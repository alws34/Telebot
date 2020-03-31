using Common.Models;
using Contracts;
using Contracts.Factories;
using SimpleInjector;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Plugins.Help
{
    [Export(typeof(IPlugin))]
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

            var result = new Response(builder.ToString());

            await respHandler(result);
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            var factory = iocContainer.GetInstance<IFactory<IPlugin>>();

            plugins = factory.GetAllEntities();
        }
    }
}
