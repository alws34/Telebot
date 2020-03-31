using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Plugins.Kill
{
    [Export(typeof(IPlugin))]
    public class KillPlugin : IPlugin
    {
        public KillPlugin()
        {
            Pattern = "/kill (\\d+)";
            Description = "Kill a task with the specified pid.";
        }

        public override async void Execute(Request req)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            Process target;

            string text = "";

            try
            {
                target = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                var result = new Response(e.Message, req.MessageId);
                await resultHandler(result);
                return;
            }

            try
            {
                target.Kill();
                text = $"{target.ProcessName} killed.";
            }
            catch (Exception e)
            {
                text = e.Message;
            }

            var result1 = new Response(text, req.MessageId);
            await resultHandler(result1);
        }
    }
}
