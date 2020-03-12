using System.Text;
using Telebot.Contracts;
using Telebot.Extensions;
using Telebot.ScreenCapture;

namespace Telebot.Commands.Status
{
    public class ScrnCapStatus : IStatus
    {
        public string GetStatus()
        {
            var result = new StringBuilder();

            var _jobs = Program.ScreenFactory.GetAllEntities();

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
