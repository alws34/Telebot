using Common.Models;
using Contracts;
using Contracts.Factories;
using Contracts.Jobs;
using CPUID.Base;
using SimpleInjector;
using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using TempTimePlugin.Jobs;
using TempTimePlugin.Models;

namespace Plugins.TempTime
{
    [Export(typeof(IPlugin))]
    public class TempTimeCommand : IPlugin
    {
        private IJob<TempArgs> worker;

        public TempTimeCommand()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";
            MinOsVersion = new Version(5, 1);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            StringBuilder text = new StringBuilder();

            worker.Update = async (s, e) =>
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

            worker.Feedback = async (s, e) =>
            {
                var result = new Response(e.Text);

                await resp(result);
            };

            string state = req.Groups[1].Value;

            if (state.Equals("off"))
            {
                var result1 = new Response("Successfully sent command \"off\" temperature monitor.");

                await resp(result1);

                worker.Stop();

                return;
            }

            var args = state.Split(' ');

            int duration = Convert.ToInt32(args[0]);
            int interval = Convert.ToInt32(args[1]);

            string text1 = $"Temperature monitor has been scheduled to run {duration} sec for every {interval} sec.";

            var response = new Response(text1);

            await resp(response);

            ((IScheduled)worker).Start(duration, interval);
        }

        public override bool GetJobActive()
        {
            return worker.Active;
        }

        public override string GetJobName()
        {
            return "Temp Time";
        }

        public override void Initialize(Container iocContainer)
        {
            var deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();

            worker = new TempSchedule(deviceFactory);
        }
    }
}
