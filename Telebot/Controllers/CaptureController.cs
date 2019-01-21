using System;
using System.Drawing;
using System.Windows.Forms;

namespace Telebot.Controllers
{
    public class CaptureController
    {
        public Bitmap CaptureDesktop()
        {
            Bitmap result = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            try
            {
                using (Graphics gObj = Graphics.FromImage(result))
                {
                    gObj.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Message:\n{e.Message}\n\nInner Message:\n{e.InnerException.Message}");
            }

            return result;
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
