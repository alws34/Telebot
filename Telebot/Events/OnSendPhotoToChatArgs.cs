using System.IO;

namespace Telebot.Events
{
    public class OnSendPhotoToChatArgs : IApplicationEvent
    {
        public OnSendPhotoToChatArgs(Stream photoStream, long chatId, int messageId)
        {
            PhotoStream = photoStream;
            ChatId = chatId;
            MessageId = messageId;
        }

        public Stream PhotoStream { get; private set; }
        public long ChatId { get; private set; }
        public int MessageId { get; private set; }
    }
}
