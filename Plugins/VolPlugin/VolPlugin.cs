using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using VolPlugin.Core;

namespace Plugins.Vol
{
    [Export(typeof(IPlugin))]
    public class VolPlugin : IPlugin
    {
        public VolPlugin()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";
            MinOsVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int vol = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response($"Successfully adjusted volume to {vol}%.");

            await resp(result);

            var api = new VolApi(vol);

            api.Invoke();
        }
    }
}
