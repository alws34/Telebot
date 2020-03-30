using Contracts;
using CPUID;
using Models;
using StatusPlugin.Statuses;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    [Export(typeof(IPlugin))]
    public class StatusPlugin : IPlugin
    {
        private IEnumerable<IStatus> statuses;

        public StatusPlugin()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.GetStatus());
            }

            var result = new Response(statusBuilder.ToString());

            await resp(result);
        }

        public override void Initialize(IPluginData entity)
        {
            var devices = CpuIdWrapper64.DeviceFactory.GetAllEntities();

            var lanPlugin = entity.Plugins.FindEntity(x => x.Pattern.StartsWith("/lan"));
            var tempPlugins = entity.Plugins.FindAll(x => x.Pattern.StartsWith("/temp"));
            var capPlugins = entity.Plugins.FindAll(x => x.Pattern.StartsWith("/captime"));

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
