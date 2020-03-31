using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID.Base;
using SimpleInjector;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

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
            MinOsVersion = new Version(5, 1);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            Spec spec = new Spec(deviceFactory);

            string info = spec.GetInfo();

            File.WriteAllText(filePath, info);

            var fileStrm = new FileStream(filePath, FileMode.Open);

            var result = new Response(fileStrm);

            await resp(result);
        }

        public override void Initialize(Container iocContainer)
        {
            deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();
        }
    }
}
