using AudioSwitcher.AudioApi.CoreAudio;

namespace Telebot.BusinessLogic
{
    public class MediaLogic
    {
        private readonly CoreAudioDevice defaultPlaybackDevice;

        public MediaLogic()
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void SetVolume(double percentage)
        {
            defaultPlaybackDevice.Volume = percentage;
        }
    }
}
