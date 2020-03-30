using Contracts;
using Models;
using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using TempTimePlugin.Jobs;
using TempTimePlugin.Models;

namespace Telebot.Commands
{
    [Export(typeof(IPlugin))]
    public class TempTimeCommand : IPlugin
    {
        private readonly IJob<TempArgs> _job;

        public TempTimeCommand()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";
            MinOSVersion = new Version(5, 1);

            _job = new TempSchedule();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            StringBuilder text = new StringBuilder();

            _job.Update = async (s, e) =>
            {
                switch (e)
                {
                    case null:
                        text.AppendLine("\nFrom *Telebot*");
                        var update = new Response(text.ToString());
                        await resp(update);
                        text.Clear();
                        break;
                    default:
                        text.AppendLine($"*{e.DeviceName}*: {e.Temperature}°C");
                        break;
                }
            };

            _job.Feedback = async (s, e) =>
            {
                var result = new Response(e.Text);

                await resp(result);
            };

            string state = req.Groups[1].Value;

            if (state.Equals("off"))
            {
                var result1 = new Response("Successfully sent command \"off\" temperature monitor.");

                await resp(result1);

                _job.Stop();

                return;
            }

            var args = state.Split(' ');

            int duration = Convert.ToInt32(args[0]);
            int interval = Convert.ToInt32(args[1]);

            string text1 = $"Temperature monitor has been scheduled to run {duration} sec for every {interval} sec.";

            var response = new Response(text1);

            await resp(response);

            ((IScheduled)_job).Start(duration, interval);
        }

        public override bool GetJobActive()
        {
            return _job.Active;
        }

        public override string GetJobName()
        {
            return "Temp Time";
        }
    }
}
