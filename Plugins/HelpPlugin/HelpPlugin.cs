using Common.Models;
using Contracts;
using Contracts.Factories;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

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
            MinOsVersion = new Version(5, 0);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            var builder = new StringBuilder();

            foreach (IPlugin plugin in plugins)
            {
                builder.AppendLine(plugin.ToString());
            }

            var result = new Response(builder.ToString());

            await resp(result);
        }

        public override void Initialize(Container iocContainer)
        {
            var factory = iocContainer.GetInstance<IFactory<IPlugin>>();

            plugins = factory.GetAllEntities();
        }
    }
}
