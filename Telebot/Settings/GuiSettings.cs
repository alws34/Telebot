using System.Collections.Generic;
using System.Drawing;

namespace Telebot.Settings
{
    public class GuiSettings
    {
        private readonly ISettings settings;

        public GuiSettings(ISettings settings)
        {
            this.settings = settings;
        }

        public Rectangle GetFormBounds()
        {
            return settings.ReadObject<Rectangle>("GUI", "Form1.Bounds");
        }

        public void SaveFormBounds(Rectangle bounds)
        {
            settings.WriteObject("GUI", "Form1.Bounds", bounds);
        }

        public List<int> GetListView1ColumnsWidth()
        {
            return settings.ReadObject<List<int>>("GUI", "listview1.Columns.Width");
        }

        public void SaveListView1ColumnsWidth(List<int> widths)
        {
            settings.WriteObject("GUI", "listview1.Columns.Width", widths);
        }
    }
}
