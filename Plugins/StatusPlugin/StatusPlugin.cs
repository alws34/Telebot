using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID.Base;
using SimpleInjector;
using StatusPlugin.Statuses;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

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
        }

        public override async void Execute(Request req)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.GetStatus());
            }

            var result = new Response(statusBuilder.ToString());

            await respHandler(result);
        }

        public override void Initialize(ResponseHandler respHandler, Container iocContainer)
        {
            base.Initialize(respHandler);

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
