using Contracts;
using System.Diagnostics;

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
            var si = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @".\sndvol64.exe",
                Arguments = $"/SetVolume Speakers {level}"
            };

            Process.Start(si);
        }
    }
}
