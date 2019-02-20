using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telebot.Loggers;

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
                int x = screen.Bounds.X;
                int y = screen.Bounds.Y;

                var result = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);

                using (Graphics graphic = Graphics.FromImage(result))
                {
                    graphic.CopyFromScreen(x, y, 0, 0, result.Size);
                }

                yield return result;
            }
        }

        public Bitmap CaptureControl(Control control)
        {
            Bitmap result = new Bitmap(control.Width, control.Height);

            control.Invoke((MethodInvoker)delegate
            {
                control.DrawToBitmap(result, new Rectangle(0, 0, control.Width, control.Height));
            });

            return result;
        }
    }
}
