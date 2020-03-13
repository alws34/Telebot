using System.Diagnostics;
using System.IO;

namespace Telebot.Intranet
{
    public class LanScanner : IInetScanner
    {
        private readonly ProcessStartInfo si;

        public LanScanner()
        {
            si = new ProcessStartInfo(
               wnetPath, $"/sxml {scanPath}"
            );
        }

        public override void Discover()
        {
            if (!File.Exists(wnetPath))
            {
                RaiseNotify($"{wnetPath} does not exist.");
                return;
            };

            Process wnet = Process.Start(si);
            wnet.WaitForExit();

            if (!File.Exists(scanPath))
            {
                RaiseNotify($"{scanPath} does not exist.");
                return;
            };

            RaiseDiscovered(ReadHosts(scanPath));
        }
    }
}
