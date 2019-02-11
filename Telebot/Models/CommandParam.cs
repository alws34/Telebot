using System.Text.RegularExpressions;
using Telebot.Commands;

namespace Telebot.Models
{
    public class CommandParam
    {
        public ICommand[] Commands { get; set; }
        public GroupCollection Groups { get; set; }
    }
}
