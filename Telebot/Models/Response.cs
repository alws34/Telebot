using System;
using System.IO;
using Telebot.Common;

namespace Telebot.Models
{

    public class Response
    {
        public string Text { get; set; }
        public Stream Raw { get; set; }
        public ResultType ResultType { get; set; }
    }
}
