using Common.Contracts;
using Common.Models;
using System;
using System.Diagnostics;

namespace Plugins.Kill
{
    public class KillPlugin : IModule
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

            string text;

            try
            {
                target = Process.GetProcessById(pid);
            }
            catch (Exception e)
            {
                var result = new Response(e.Message, req.MessageId);
                await ResultHandler(result);
                return;
            }

            try
            {
                text = $"{target.ProcessName} killed.";
                target.Kill();
            }
            catch (Exception e)
            {
                text = e.Message;
            }

            var result1 = new Response(text, req.MessageId);
            await ResultHandler(result1);
        }
    }
}
