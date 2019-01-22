using System;
using System.Drawing;
using System.Windows.Forms;
using Telebot.Contracts;

namespace Telebot.Controllers
{
    public class CaptureController
    {
        private readonly ILogger logger;

        public CaptureController()
        {
            logger = Program.container.GetInstance<ILogger>();
        }

        public Bitmap CaptureDesktop()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            Bitmap result = new Bitmap(width, height);

            try
            {
                using (Graphics gObj = Graphics.FromImage(result))
                {
                    gObj.CopyFromScreen(0, 0, 0, 0, result.Size);
                }
            }
            catch (Exception e)
            {
                logger.Log(e.ToString());
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
