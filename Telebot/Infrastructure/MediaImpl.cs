using System.Diagnostics;

namespace Telebot.Infrastructure
{
    public class MediaImpl
    {
        public void SetVolume(int percentage)
        {
            var si = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @".\SetVol.exe",
                Arguments = $"{percentage}"
            };

            Process.Start(si);
        }
    }
}
