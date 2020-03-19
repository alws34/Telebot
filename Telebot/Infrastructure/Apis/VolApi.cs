using System.Diagnostics;

namespace Telebot.Infrastructure.Apis
{
    public class VolApi : IApi
    {
        private readonly int volume;

        public VolApi(int volume)
        {
            this.volume = volume;

            Action = SetVolume;
        }

        public void SetVolume()
        {
            var si = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @".\SetVol.exe",
                Arguments = $"{volume}"
            };

            Process.Start(si);
        }
    }
}
