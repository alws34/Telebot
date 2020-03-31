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

            await respHandler(result);
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();
        }
    }
}
