using System.Diagnostics;
using System.IO;

namespace Telebot.Intranet
{
    public class LanScanner : IInetScanner
    {
        public override void Discover()
        {
            if (!File.Exists(utilPath))
            {
                RaiseNotify($"{utilPath} does not exist.");
                return;
            };

            var si = new ProcessStartInfo(
                utilPath, $"/sxml {scanPath}"
            );

            Process exc = Process.Start(si);
            exc.WaitForExit();

            if (!File.Exists(scanPath))
            {
                RaiseNotify($"{scanPath} does not exist.");
                return;
            };

            RaiseDiscovered(GetHosts());
        }
    }
}
