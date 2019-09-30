using Telebot.Clients;
using Telebot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureAggregator
    {
        private readonly ITelebotClient client;

        public ScreenCaptureAggregator(ITelebotClient client, IScreenCapture screenCapture)
        {
            this.client = client;

            screenCapture.ScreenCaptured += ScreenCaptured;
        }

        private async void ScreenCaptured(object sender, ScreenCaptureArgs e)
        {
            var document = new InputOnlineFile(e.Capture.ToStream(), "captime.jpg");

            await client.SendDocumentAsync(client.AdminID, document, thumb: document as InputMedia);
        }
    }
}
