using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static Telebot.Native.User32;

namespace Telebot.Infrastructure
{
    public class DesktopApi
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
            bool isWindowMinized(IntPtr wnd)
            {
                IntPtr wndStyles = GetWindowLong(wnd, (int)GWL.GWL_STYLE);

                return (wndStyles.ToInt64() & WS_MINIMIZE) != 0;
            }

            bool minimize = false;

            if (isWindowMinized(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
                minimize = true;
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

            if (minimize)
                ShowWindow(hWnd, SW_MINIMIZE);

            return result;
        }
    }
}
