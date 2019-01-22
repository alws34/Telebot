using System.IO;
using Telebot.Contracts;

namespace Telebot.Loggers
{
    public class FileLogger : ILogger
    {
        public void Log(string text)
        {
            File.WriteAllText("Exceptions.txt", text);
        }
    }
}
