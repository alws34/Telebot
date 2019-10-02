namespace Telebot.Commands.Status
{
    public class CaptureStatus : IStatus
    {
        public string Execute()
        {
            string BoolToStr(bool condition)
            {
                return condition ? "Active" : "Inactive";
            }

            var screenCapture = Program.screenCaptureSchedule;

            string active = BoolToStr(screenCapture.IsActive);

            return $"*Schedule* 🖼️: {active}";
        }
    }
}
