using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Telebot.Network
{
    public class LanMonitor : INetMonitor
    {
        private const string utilPath = ".\\WNetWatcher.exe";
        private const string scanPath = ".\\scan.xml";

        public override void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public override void Discover()
        {
            var si = new ProcessStartInfo(
                utilPath, $"/sxml {scanPath}"
            );

            Process exc = Process.Start(si);
            exc.WaitForExit();

            RaiseDiscoveredHosts(ParseOutput());
        }

        private HostsArg ParseOutput()
        {
            using (var fileStream = new FileStream(scanPath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(HostsArg));

                return (HostsArg)serializer.Deserialize(fileStream);
            }
        }

        public override void Listen()
        {
            throw new System.NotImplementedException();
        }
    }
}
