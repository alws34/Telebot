using System.Drawing;

namespace Telebot.Events
{
    public class OnScreenCaptureArgs : IApplicationEvent
    {
        public OnScreenCaptureArgs(Bitmap photo)
        {
            Photo = photo;
        }
        public Bitmap Photo { get; set; }
    }
}
