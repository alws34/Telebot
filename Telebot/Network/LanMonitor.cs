using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Telebot.Network
{
    public class LanMonitor : INetMonitor
    {
        private const string dir = ".\\wnetwatcher";

        private readonly string utilPath;
        private readonly string outputPath;

        public LanMonitor()
        {
            utilPath = $"{dir}\\WNetWatcher.exe";
            outputPath = $"{dir}\\output.xml";
        }

        public override void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public override void Discover()
        {
            var si = new ProcessStartInfo(
                utilPath, $"/sxml {outputPath}"
            );

            Process exc = Process.Start(si);
            exc.WaitForExit();

            var hosts = ParseOutput(outputPath);

            RaiseDiscoveredHosts(hosts);
        }

        private HostsArg ParseOutput(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
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
