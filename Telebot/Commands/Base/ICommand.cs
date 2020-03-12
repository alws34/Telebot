using System;
using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Commands
{
    public abstract class ICommand
    {
        public string Pattern { get; protected set; }

        public string Description { get; protected set; }

        public abstract void Execute(Request info, Func<Response, Task> cbResult);

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
