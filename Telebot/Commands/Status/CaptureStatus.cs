using System.Text;
using Telebot.ScreenCapture;

namespace Telebot.Commands.Status
{
    public class CaptureStatus : IStatus
    {
        private readonly IScreenCapture[] screenCaptures;

        public CaptureStatus()
        {
            screenCaptures = Program.screenCapFactory.GetAllEntities();
        }

        public string Execute()
        {
            string BoolToStr(bool condition)
            {
                return condition ? "Active" : "Inactive";
            }

            var result = new StringBuilder();

            foreach (IScreenCapture screenCap in screenCaptures)
            {
                string name = screenCap.GetType().Name.Replace("ScreenCapture", "");
                string active = BoolToStr(screenCap.IsActive);
                result.AppendLine($"*{name}* 🖼️: {active}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
