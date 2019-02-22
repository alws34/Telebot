using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Telebot.Helpers.User32Helper;

namespace Telebot.BusinessLogic
{
    public class CaptureLogic
    {
        public IEnumerable<Bitmap> CaptureDesktop()
        {
            //int width = SystemInformation.VirtualScreen.Width;
            //int height = SystemInformation.VirtualScreen.Height;
            //int left = SystemInformation.VirtualScreen.Left;
            //int top = SystemInformation.VirtualScreen.Top;

            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;

            //Bitmap result = new Bitmap(width, height);

            foreach (Screen screen in Screen.AllScreens)
            {
                int left = screen.Bounds.Left;
                int top = screen.Bounds.Top;

                int width = screen.Bounds.Width;
                int height = screen.Bounds.Height;

                var result = new Bitmap(width, height);

                using (var graphic = Graphics.FromImage(result))
                {
                    graphic.CopyFromScreen(left, top, 0, 0, result.Size);
                }

                yield return result;
            }
        }

        public Bitmap CaptureWindow(IntPtr hWnd)
        {
            var rect = new Rect();

            GetWindowRect(hWnd, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var result = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(rect.left, rect.top, 0, 0, result.Size);
            }

            return result;
        }
    }
}
