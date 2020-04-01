using Common.Models;
using Contracts;
using System;
using VolPlugin.Core;

namespace Plugins.Vol
{

    public class VolPlugin : IPlugin
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
                $"Successfully adjusted volume to {vol}%.",
                req.MessageId
            );

            await ResultHandler(result);

            var api = new VolApi(vol);

            api.Invoke();
        }
    }
}
