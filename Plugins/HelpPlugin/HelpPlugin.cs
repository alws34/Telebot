using Contracts;
using Models;
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
            Description = "List of available plugins.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var builder = new StringBuilder();

            var plugins = entity.Plugins.GetAllEntities();

            foreach (IPlugin plugin in plugins)
            {
                builder.AppendLine(plugin.ToString());
            }

            var result = new Response(builder.ToString());

            await resp(result);
        }

        public override void Initialize(IPluginData entity)
        {
            this.entity = entity;
        }
    }
}
