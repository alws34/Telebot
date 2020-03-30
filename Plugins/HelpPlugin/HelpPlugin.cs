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
        private Func<Response, Task> responseHandler;

        public HelpPlugin()
        {
            Pattern = "/help";
            Description = "List of available plugins.";
            MinOSVersion = new Version(5, 0);

            MessageHub.MessageHub.Instance.Subscribe<IpcPlugin>(IpcPluginHandler);
        }

        private async void IpcPluginHandler(IpcPlugin e)
        {
            var s = new StringBuilder();

            foreach (IPlugin plugin in e.Plugins)
            {
                s.AppendLine(plugin.ToString());
            }

            var result = new Response(s.ToString());

            await responseHandler(result);
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            if (responseHandler == null)
                responseHandler = resp;

            MessageHub.MessageHub.Instance.Publish(new IpcPluginsGet());
        }
    }
}
