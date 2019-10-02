using System.Diagnostics;

namespace Telebot.CoreApis
{
    public class MediaApi
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
