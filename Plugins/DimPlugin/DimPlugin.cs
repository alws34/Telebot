using Common.Models;
using DimPlugin.Core;
using System;

namespace Plugins.Dim
{

    public class DimPlugin : IPlugin
    {
        public DimPlugin()
        {
            Pattern = "/dim (\\d{1,3})";
            Description = "Adjust workstation's brightness.";
        }

        public override async void Execute(Request req)
        {
            int level = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response(
                $"Successfully set brightness to {level}%.",
                req.MessageId
            );

            await ResultHandler(result);

            var api = new DimApi(level);

            api.Invoke();
        }
    }
}
