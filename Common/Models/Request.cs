﻿using System.Text.RegularExpressions;

namespace Common.Models
{
    public class Request
    {
        public GroupCollection Groups { get; set; }
        public int MessageId { get; set; }
    }
}
