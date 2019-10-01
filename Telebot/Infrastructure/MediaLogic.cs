using System.Diagnostics;

namespace Telebot.Infrastructure
{
    public class MediaLogic
    {
        public void SetVolume(int percentage)
        {
            Process.Start(@".\SoundVolumeView.exe", $"/SetVolume Speakers {percentage}");
        }
    }
}
