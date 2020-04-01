using Common;
using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID.Base;
using StatusPlugin.Statuses;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Plugins.Status
{
    [Export(typeof(IPlugin))]
    public class StatusPlugin : IPlugin
    {
        private IEnumerable<IStatus> items { get; set; }

        public StatusPlugin()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";
        }

        public override async void Execute(Request req)
        {
            var builder = new StringBuilder();

            foreach (IStatus status in items)
            {
                builder.AppendLine(status.GetStatus());
            }

            var result = new Response(
                builder.ToString(),
                req.MessageId
            );

            await ResultHandler(result);
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            var statusInstance = data.iocContainer.GetInstance<IFactory<IStatus>>();
            var deviceInstance = data.iocContainer.GetInstance<IFactory<IDevice>>();

            IStatus[] arr =
            {
                new SysInfo(deviceInstance.GetAll()),
                new LanIp(),
                new WanIp(),
                new Uptime()
            };

            items = arr.Concat(statusInstance.GetAll());
        }
    }
}
