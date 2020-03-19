using System.Diagnostics;

namespace Telebot.Infrastructure.Apis
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
                FileName = @".\SetVol.exe",
                Arguments = $"{level}"
            };

            Process.Start(si);
        }
    }
}
