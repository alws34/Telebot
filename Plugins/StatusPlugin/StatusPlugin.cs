﻿using Common.Contracts;
using Common.Models;
using CPUID.Base;
using StatusPlugin.Statuses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;

namespace Plugins.Status
{
    public class StatusPlugin : IModule
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

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            var container = (Container)data.IoCProvider.GetService(typeof(Container));

            var devices = container.GetAllInstances<IDevice>();

            IStatus[] classStatuses =
            {
                new SysInfo(devices),
                new LanIp(),
                new WanIp(),
                new Uptime()
            };

            var jobStatuses = container.GetAllInstances<IJobStatus>();

            items = classStatuses.Concat(jobStatuses);
        }
    }
}
