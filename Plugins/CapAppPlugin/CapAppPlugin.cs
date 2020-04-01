using CapAppPlugin.Core;
using Common.Contracts;
using Common.Extensions;
using Common.Models;
using System;
using System.Diagnostics;

namespace Plugins.CapApp
{

    public class CapAppPlugin : IModule
    {
        public CapAppPlugin()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of an app (by pid).";
        }

        public override void Execute(Request req)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var api = new CapApi(hWnd);

            api.Invoke(async (wnd) =>
            {
                var result = new Response(wnd.ToMemStream(), req.MessageId);

                await ResultHandler(result);
            });
        }
    }
}
