using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID.Base;
using SimpleInjector;
using StatusPlugin.Statuses;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Status
{
    [Export(typeof(IPlugin))]
    public class StatusPlugin : IPlugin
    {
        private IEnumerable<IStatus> statuses;

        public StatusPlugin()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";
            MinOsVersion = new Version(5, 0);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.GetStatus());
            }

            var result = new Response(statusBuilder.ToString());

            await resp(result);
        }

        public override void Initialize(Container iocContainer)
        {
            var deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();

            var devices = deviceFactory.GetAllEntities();

            var pluginFactory = iocContainer.GetInstance<IFactory<IPlugin>>();

            var lanPlugin = pluginFactory.FindEntity(x => x.Pattern.StartsWith("/lan"));
            var tempPlugins = pluginFactory.FindAll(x => x.Pattern.StartsWith("/temp"));
            var capPlugins = pluginFactory.FindAll(x => x.Pattern.StartsWith("/captime"));

            statuses = new IStatus[]
            {
                new SystemStatus(devices),
                new LanAddrStatus(),
                new WanAddrStatus(),
                new UptimeStatus(),
                new LanMonStatus(lanPlugin),
                new TempStatus(tempPlugins),
                new CapsStatus(capPlugins)
            };
        }
    }
}
