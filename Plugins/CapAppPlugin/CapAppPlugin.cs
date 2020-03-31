using CapAppPlugin.Core;
using Common.Extensions;
using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Plugins.CapApp
{
    [Export(typeof(IPlugin))]
    public class CapAppPlugin : IPlugin
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

                await resultHandler(result);
            });
        }
    }
}
