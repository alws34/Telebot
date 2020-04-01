using Common.Contracts;
using Common.Models;
using CPUID.Base;
using System.Collections.Generic;
using System.IO;

namespace Plugins.NSSpec
{

    public class SpecPlugin : IPlugin
    {
        private const string filePath = ".\\Plugins\\Spec\\spec.txt";

        private IEnumerable<IDevice> devices;

        public SpecPlugin()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public override async void Execute(Request req)
        {
            Spec spec = new Spec(devices);

            string info = spec.GetInfo();

            File.WriteAllText(filePath, info);

            var fileHandle = new FileStream(filePath, FileMode.Open);

            var result = new Response(fileHandle, req.MessageId);

            await ResultHandler(result);
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            devices = data.IocContainer.GetAllInstances<IDevice>();
        }
    }
}
