using System.Text.RegularExpressions;

namespace BotSdk.Models
{
    public class Request
    {
        public GroupCollection Groups { get; set; }
        public int MessageId { get; set; }
    }
}
