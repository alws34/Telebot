using System.Text;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.ScreenCapture;

namespace Telebot.Commands.Status
{
    public class CaptureStatus : IStatus
    {
        private readonly IJob<ScreenCaptureArgs>[] _jobs;

        public CaptureStatus()
        {
            _jobs = Program.ScreenFactory.GetAllEntities();
        }

        public string Execute()
        {
            var result = new StringBuilder();

            foreach (IJob<ScreenCaptureArgs> job in _jobs)
            {
                string name = job.GetType().Name.Replace("ScreenCapture", "");
                string active = job.IsActive.AsReadable();
                result.AppendLine($"*{name}* 🖼️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
