using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Telebot.Extensions
{
    public static class BitmapExtensions
    {
        public static Stream ToStream(this Bitmap bitmap)
        {
            var result = new MemoryStream();
            bitmap.Save(result, ImageFormat.Png);
            result.Position = 0;
            return result;
        }
    }
}
