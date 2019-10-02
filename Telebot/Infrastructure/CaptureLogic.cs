using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static Telebot.Helpers.User32Helper;

namespace Telebot.Infrastructure
{
    public class CaptureLogic
    {
        private const byte SW_RESTORE = 9;
        private const byte SW_MINIMIZE = 6;

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

                Bitmap result = new Bitmap(width, height);

                using (var graphic = Graphics.FromImage(result))
                {
                    graphic.CopyFromScreen(left, top, 0, 0, result.Size);
                }

                yield return result;
            }
        }

        public Bitmap CaptureWindow(IntPtr hWnd)
        {
            if (isWindowMinized(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }
            else
            {
                SetForegroundWindow(hWnd);
            }

            Thread.Sleep(250);

            Rect rect = new Rect();

            GetWindowRect(hWnd, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            Bitmap result = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(rect.left, rect.top, 0, 0, result.Size);
            }

            return result;
        }

        private bool isWindowMinized(IntPtr hWnd)
        {
            const uint WS_MINIMIZE = 0x20000000;

            IntPtr wndStyles = GetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE);

            return (wndStyles.ToInt64() & WS_MINIMIZE) != 0;
        }
    }
}
