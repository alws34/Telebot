namespace Telebot.Events
{
    public class HighTemperatureMessage : IApplicationEvent
    {
        public string Message { get; private set; }

        public HighTemperatureMessage(string message)
        {
            Message = message;
        }
    }
}
