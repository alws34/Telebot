using System.Collections.Generic;
using System.Xml.Serialization;

namespace Telebot.Network
{
    [XmlRoot(ElementName = "devices_connected_to_your_network")]
    public class HostsArg
    {
        [XmlElement(ElementName = "item")]
        public List<Host> Hosts { get; set; }
    }
}