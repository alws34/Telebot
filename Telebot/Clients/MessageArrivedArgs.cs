using System;

namespace Telebot.Clients
{
    public class MessageArrivedArgs : EventArgs
    {
        public string MessageText { get; set; }
    }
}
