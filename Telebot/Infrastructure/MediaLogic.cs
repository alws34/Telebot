﻿using System.Diagnostics;

namespace Telebot.Infrastructure
{
    public class MediaLogic
    {
        public void SetVolume(int percentage)
        {
            var si = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @".\SetVol.exe",
                Arguments = $"{percentage}"
            };

            Process.Start(si);
        }
    }
}
