using System.Text;
using Telebot.Capture;
using Telebot.Contracts;
using Telebot.Extensions;

namespace Telebot.Commands.Status
{
    public class CapsStatus : IStatus
    {
        public string GetStatus()
        {
            var result = new StringBuilder();

            var _jobs = Program.CaptureFactory.GetAllEntities();

            foreach (IJob<CaptureArgs> job in _jobs)
            {
                string name = job.GetType().Name.Replace("ScreenCapture", "");
                string active = job.IsActive.AsReadable();
                result.AppendLine($"*{name}* 🖼️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
