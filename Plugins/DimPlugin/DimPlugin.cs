using Contracts;
using DimPlugin.Core;
using Common.Models;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace DimPlugin
{
    [Export(typeof(IPlugin))]
    public class DimPlugin : IPlugin
    {
        public DimPlugin()
        {
            Pattern = "/dim (\\d{1,3})";
            Description = "Adjust workstation's brightness.";
            MinOSVersion = new Version(6, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int level = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response($"Successfully set brightness to {level}%.");

            await resp(result);

            var api = new DimApi(level);

            api.Invoke();
        }
    }
}
