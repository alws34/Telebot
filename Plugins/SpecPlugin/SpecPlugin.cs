using BotSdk.Contracts;
using BotSdk.Models;
using System.IO;

namespace Plugins.NSSpec
{
    public class SpecPlugin : IModule
    {
        private const string filePath = ".\\Plugins\\Spec\\spec.txt";

        private Spec spec;

        public SpecPlugin()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public override async void Execute(Request req)
        {
            string info = spec.GetInfo();

            File.WriteAllText(filePath, info);

            var fileHandle = new FileStream(filePath, FileMode.Open);

            var result = new Response(fileHandle, req.MessageId);

            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);
            spec = new Spec(data.IoCProvider);
        }
    }
}
