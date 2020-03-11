using System;

namespace Telebot.Clients
{
    public class ReceivedArgs : EventArgs
    {
        public string MessageText { get; set; }
    }
}
