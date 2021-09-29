using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TShockAPI;

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
        public static bool TryParseJson<T>(string text, out T json)
        {
            try
            {
                json = JsonConvert.DeserializeObject<T>(text);
                return true;
            }
            catch
            {
                json = default;
                return false;
            }
        }
        public static T StyleEX<T>(this UIStyle style) where T : UIStyle, new()
        {
            if (style == null)
                return default;
            var s = new T();
            s.Stratify(style);
            return s;
        }
        public static TSPlayer Player(this Touch t) => TShock.Players[t.PlayerIndex];
    }
}
