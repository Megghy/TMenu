using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Widgets;

namespace TMenu
{
    public static class Utils
    {
        public static void UpdateText(this Label l, object text)
        {
            l?.SetText(text.ToString());
            l?.UpdateSelf();
        }
        public static void UpdateTextColor(this Label l, byte id)
        {
            l.LabelStyle.TextColor = id;
            l?.UpdateSelf();
        }
        public static void UpdateTileColor(this VisualObject l, byte id)
        {
            l.Style.TileColor = id;
            l?.UpdateSelf();
        }
        public static void UpdateWallColor(this VisualObject l, byte id)
        {
            l.Style.WallColor = id;
            l?.UpdateSelf();
        }
        public static void UpdateSelf(this VisualObject v) => v.Update().Apply().Draw();
    }
}
