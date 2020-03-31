using AppsPlugin.Core;
using AppsPlugin.Enums;
using Common.Models;
using Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Plugins.Apps
{
    [Export(typeof(IPlugin))]
    public class AppsPlugin : IPlugin
    {
        private Dictionary<string, Session> types;

        public AppsPlugin()
        {
            Pattern = "/apps (fg|all)";
            Description = "List of active applications.";

            types = new Dictionary<string, Session>
            {
                { "fg", Session.Foreground },
                { "all", Session.Background }
            };
        }

        public override void Execute(Request req)
        {
            string key = req.Groups[1].Value;

            Session type = types[key];

            var api = new AppsApi(type);

            api.Invoke(async s =>
            {
                var result = new Response(s);

                await respHandler(result);
            });
        }
    }
}
