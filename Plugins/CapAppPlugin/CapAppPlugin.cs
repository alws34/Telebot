using BotSdk.Contracts;
using BotSdk.Extensions;
using BotSdk.Models;
using CapAppPlugin.Core;
using System;
using System.Diagnostics;

namespace Plugins.CapApp
{

    public class CapAppPlugin : IModule
    {
        public CapAppPlugin()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of an app (by pid).";
        }

        public override async void Execute(Request req)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            Process task;

            try
            {
                task = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                var resp = new Response(e.Message, req.MessageId);
                await ResultHandler(resp);
                return;
            }

            var api = new CapApi(task.MainWindowHandle);

            api.Invoke(async (wnd) =>
            {
                var result = new Response(wnd.ToMemStream(), req.MessageId);

                await ResultHandler(result);
            });
        }
    }
}
