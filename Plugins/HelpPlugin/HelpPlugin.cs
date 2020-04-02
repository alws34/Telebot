using Common.Contracts;
using Common.Models;
using System.Text;

namespace Plugins.Help
{
    public class HelpPlugin : IModule
    {
        private string text;

        public HelpPlugin()
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

            var modules = data.IocContainer.GetAllInstances<IModule>();

            var builder = new StringBuilder();

            foreach (IModule module in modules)
            {
                builder.AppendLine(module.ToString());
            }

            text = builder.ToString();
        }
    }
}
