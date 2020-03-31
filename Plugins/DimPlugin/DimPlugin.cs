using Common.Models;
using Contracts;
using DimPlugin.Core;
using System;
using System.ComponentModel.Composition;

namespace Plugins.Dim
{
    [Export(typeof(IPlugin))]
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

            var result = new Response($"Successfully set brightness to {level}%.");

            await respHandler(result);

            var api = new DimApi(level);

            api.Invoke();
        }
    }
}
