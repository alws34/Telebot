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
        private readonly IEnumerable<IStatus> statuses;

        public StatusPlugin()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";
            MinOSVersion = new Version(5, 0);

            var devices = CpuIdWrapper64.DeviceFactory.GetAllEntities();

            statuses = new IStatus[]
            {
                new SystemStatus(devices),
                new LanAddrStatus(),
                new WanAddrStatus(),
                new UptimeStatus(),
                new LanMonStatus(),
                new TempStatus(),
                new CapsStatus()
            };
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
    }
}
