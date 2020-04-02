using BotSdk.Contracts;
using BotSdk.Models;
using SimpleInjector;
using System.Text;

namespace Plugins.Help
{
    public class DllMain : IModule
    {
        private string text;

        public DllMain()
        {
            Pattern = "/help";
            Description = "List of available plugins.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response(
                text,
                req.MessageId
            );

            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            var container = (Container)data.IoCProvider.GetService(typeof(Container));

            var modules = container.GetAllInstances<IModule>();

            var builder = new StringBuilder();

            foreach (IModule module in modules)
            {
                builder.AppendLine(module.ToString());
            }

            text = builder.ToString();
        }
    }
}
