using Contracts;
using Common.Models;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugins.Kill
{
    [Export(typeof(IPlugin))]
    public class KillPlugin : IPlugin
    {
        public KillPlugin()
        {
            Pattern = "/kill (\\d+)";
            Description = "Kill a task with the specified pid.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
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
                var result = new Response(e.Message);
                await resp(result);
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

            var result1 = new Response(text);
            await resp(result1);
        }
    }
}
