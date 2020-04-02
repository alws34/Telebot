using BotSdk.Contracts;
using BotSdk.Models;
using System;
using VolPlugin.Core;

namespace Plugins.Vol
{
    public class DllMain : IModule
    {
        public DllMain()
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
