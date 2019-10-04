using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public class ScreenCapFactory : IFactory<IScreenCapture>
    {
        private readonly List<IScreenCapture> screenCaptures;

        public ScreenCapFactory()
        {
            screenCaptures = new List<IScreenCapture>
            {
                { new ScreenCaptureSchedule() }
            };
        }

        public IScreenCapture FindEntity(Predicate<IScreenCapture> predicate)
        {
            return screenCaptures.SingleOrDefault(x => predicate(x));
        }

        public IScreenCapture[] GetAllEntities()
        {
            return screenCaptures.ToArray();
        }
    }
}
