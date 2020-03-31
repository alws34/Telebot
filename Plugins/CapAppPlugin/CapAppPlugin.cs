using CapAppPlugin.Core;
using Common.Extensions;
using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugins.CapApp
{
    [Export(typeof(IPlugin))]
    public class CapAppPlugin : IPlugin
    {
        public CapAppPlugin()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified app (by pid).";
            MinOsVersion = new Version(5, 0);
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var api = new CapApi(hWnd);

            api.Invoke(async (wnd) =>
            {
                var result = new Response(wnd.ToMemStream());

                await resp(result);
            });
        }
    }
}
