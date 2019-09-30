using AudioSwitcher.AudioApi.CoreAudio;

namespace Telebot.Infrastructure
{
    public class MediaLogic
    {
        private readonly CoreAudioController coreAudioController;

        public MediaLogic()
        {
            coreAudioController = new CoreAudioController();
        }

        public async void SetVolume(double percentage)
        {
            await coreAudioController.DefaultPlaybackDevice.SetVolumeAsync(percentage);
        }
    }
}
