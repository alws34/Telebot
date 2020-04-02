using System.Diagnostics;
using BotSdk.Contracts;

namespace VolPlugin.Core
{
    public class VolApi : IApi
    {
        private readonly int level;

        public VolApi(int level)
        {
            this.level = level;

            Action = SetVolume;
        }

        public void SetVolume()
        {
            var si = new ProcessStartInfo(
                ".\\Plugins\\Vol\\sndvol64.exe",
                $"/SetVolume Speakers {level}"
            )
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            Process.Start(si);
        }
    }
}
