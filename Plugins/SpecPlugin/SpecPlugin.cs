using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID.Base;
using SimpleInjector;
using System.ComponentModel.Composition;
using System.IO;

namespace Plugins.NSSpec
{
    [Export(typeof(IPlugin))]
    public class SpecPlugin : IPlugin
    {
        private const string filePath = ".\\Plugins\\Spec\\spec.txt";

        private IFactory<IDevice> deviceFactory;

        public SpecPlugin()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
        }

        public override async void Execute(Request req)
        {
            Spec spec = new Spec(deviceFactory);

            string info = spec.GetInfo();

            File.WriteAllText(filePath, info);

            var fileStrm = new FileStream(filePath, FileMode.Open);

            var result = new Response(fileStrm);

            await resultHandler(result);
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            deviceFactory = data.iocContainer.GetInstance<IFactory<IDevice>>();
        }
    }
}
