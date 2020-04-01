using Common.Contracts;
using Common.Models;
using CPUID.Base;
using System.Collections.Generic;
using System.IO;
using SimpleInjector;

namespace Plugins.NSSpec
{

    public class SpecPlugin : IPlugin
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

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            spec = new Spec(data.IocContainer);
        }
    }
}
