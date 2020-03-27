using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Telebot.Infrastructure.Apis
{
    public class DesktopApi : IApi<IEnumerable<Bitmap>>
    {
        public DesktopApi()
        {
            Func = GetDesktopBitmap;
        }

        public IEnumerable<Bitmap> GetDesktopBitmap()
        {
            var items = new List<Bitmap>();

            foreach (Screen screen in Screen.AllScreens)
            {
                int left = screen.Bounds.Left;
                int top = screen.Bounds.Top;

                int width = screen.Bounds.Width;
                int height = screen.Bounds.Height;

                Bitmap result = new Bitmap(width, height);

                using (var graphic = Graphics.FromImage(result))
                {
                    graphic.CopyFromScreen(left, top, 0, 0, result.Size);
                }

                items.Add(result);
            }

            return items;
        }
    }
}
