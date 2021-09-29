using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TMenu.Controls;
using TShockAPI;

namespace TMenu
{
    public class Data
    {
        public static List<TPanel> Menus { get; set; } = new();
        public class Click
        {
            public string[] Command { get; set; }
            public string Goto { get; set; }
            public string Message { get; set; }
        }
        public class FileData
        {
            public FileData(string name, string x, string y, string width, string height, string text = null, UIConfiguration configuration = null, UIStyle style = null, Click clickCommand = null)
            {
                Name = name;
                _x = x;
                _y = y;
                _width = width;
                _height = height;
                Text = text ?? String.Empty;
                Configuration = configuration ?? new()
                {
                    UseBegin = true,
                    UseEnd = true,
                    UseMoving = true
                };
                Style = style;
                ClickCommand = clickCommand;
            }
            public string Name { get; set; }
            string _x;
            public int X
            {
                get => ParsePosition(_x);
            }
            string _y;
            public int Y => ParsePosition(_y);
            string _width;
            public int Width => ParsePosition(_width);
            string _height;
            public int Height => ParsePosition(_height);
            public string Text { get; set; }
            public UIConfiguration Configuration { get; set; }
            public UIStyle Style { get; set; }
            public Click ClickCommand { get; set; }
            public TSPlayer Player { get; set; }
            public static bool TryParseFromJson(JToken json, out FileData data)
            {
                try
                {
                    data = new FileData(
                        json.Value<string>("name"),
                        json.Value<string>("x"),
                        json.Value<string>("y"),
                        json.Value<string>("width"),
                        json.Value<string>("height"),
                        json.Value<string>("text"),
                        json.Value<JObject>("configuration")?.ToObject<UIConfiguration>(),
                        json.Value<JObject>("style")?.ToObject<UIStyle>(),
                        json.Value<JObject>("click")?.ToObject<Click>()
                        );
                    return true;
                }
                catch
                {
                    data = null;
                    return false;
                }
            }
            public int ParsePosition(string text)
            {
                var i = 0;
                return int.TryParse(text, out i) ? i : 0;
                Type scriptType = Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));

                dynamic obj = Activator.CreateInstance(scriptType, false);
                obj.Language = "javascript";

                return obj.Eval(text) ?? int.TryParse(text, out i) ? i : 0;
            }
            string ParseVariable(string text)
            {
                return text
                    .Replace("{player.x}", Player.TileX.ToString());
            }
        }
    }
}
