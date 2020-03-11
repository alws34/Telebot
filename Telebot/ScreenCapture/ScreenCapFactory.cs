using Telebot.Common;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public class ScreenCapFactory : Factory<IJob<ScreenCaptureArgs>>
    {
        public ScreenCapFactory()
        {
            _items.Add(new ScreenCaptureSchedule());
        }
    }
}
