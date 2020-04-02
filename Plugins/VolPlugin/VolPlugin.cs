using System;
using BotSdk.Contracts;
using BotSdk.Models;
using VolPlugin.Core;

namespace Plugins.Vol
{
    public class VolPlugin : IModule
    {
        public VolPlugin()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";
        }

        public override async void Execute(Request req)
        {
            int vol = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response(
                $"Successfully set volume to {vol}%.",
                req.MessageId
            );

            await ResultHandler(result);

            var api = new VolApi(vol);

            api.Invoke();
        }
    }
}
