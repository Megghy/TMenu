using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets.Data;
using TMenu.Controls;
using TShockAPI;

namespace TMenu
{
    public class Data
    {
        public static List<TPanel> Menus { get; set; } = new();
        public static Dictionary<Type, string> ControlName { get; set; } = new()
        {
            { typeof(TButton), "button" },
            { typeof(TContainer), "container" },
            { typeof(TSign), "sign" },
            { typeof(TChest), "chest" },
            { typeof(TArror), "arror" },
            { typeof(TItemRack), "itemrack" },
        };
        public static void Init()
        {
            ControlName.Clear();
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "TMenu.Controls" && t.BaseType.Name == "TMenuControlBase`1")
                .ForEach(t => ControlName.Add(t, t.GetCustomAttribute<NameInJsonAttribute>() is { } a ? a.Name : t.Name.Remove(0, 1).ToLower()));
        }
        public class Variables
        {
            public class VariableNameAttribute : Attribute
            {
                public VariableNameAttribute(string name)
                {
                    Name = name;
                }
                public string Name { get; set; }
            }
            public Variables(TSPlayer plr)
            {
                Player = plr;
            }
            public TSPlayer Player { get; private set; }
            [VariableName("player.x")]
            public float X => Player.X;
            [VariableName("player.y")]
            public float Y => Player.Y;
            [VariableName("player.tilex")]
            public int TileX => Player.TileX;
            [VariableName("player.tiley")]
            public int TileY => Player.TileY;
            public string ReflectValue(string name)
            {
                //todo
                return "";
            }
        }
        public class Click
        {
            public string[] Command { get; set; }
            public string Goto { get; set; }
            public string Message { get; set; }
        }
        public class FileData
        {
            public FileData() : this("unknown", "0", "0", "0", "0")
            {
            }
            public FileData(string name, string x, string y, string width, string height, string text = null, UIConfiguration configuration = null, UIStyle style = null, Click clickCommand = null)
            {
                Name = name;
                _x = x;
                _y = y;
                _width = width;
                _height = height;
                Text = text ?? String.Empty;
                Config = configuration ?? new()
                {
                    UseBegin = true,
                    UseEnd = true,
                    UseMoving = true
                };
                Style = style;
                ClickCommand = clickCommand;
            }
            public string Path { get; set; }
            public string Name { get; set; }
            string _x;
            public int X => ParsePosition(_x);
            string _y;
            public int Y => ParsePosition(_y);
            string _width;
            public int Width => ParsePosition(_width);
            string _height;
            public int Height => ParsePosition(_height);
            public string Text { get; set; }
            public UIConfiguration Config { get; set; }
            public UIStyle Style { get; set; }
            public Click ClickCommand { get; set; } = new();
            #region 一些不一定存在的属性
            public Direction Direction { get; set; } = Direction.Up;
            internal ItemData[] _items;
            public ItemData Item { get; set; } = new();
            public ItemData[] Items { get; set; } = new ItemData[0];
            public bool Personal { get; set; } = true;
            public bool Moveable { get; set; } = true;
            #endregion
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
                        json.Value<JObject>("config")?.ToObject<UIConfiguration>(),
                        json.Value<JObject>("style")?.ToObject<UIStyle>(),
                        json.Value<JObject>("click")?.ToObject<Click>()
                        )
                    {
                        _items = json.Value<JArray>("items")?.ToObject<ItemData[]>(),
                        Direction = Enum.TryParse<Direction>(json.Value<JObject>("style")?.Value<string>("direction") ?? "", out var d) ? d : Direction.Up,
                        Moveable = json.Value<bool>("moveable"),
                        Personal = json.Value<bool>("public"),
                    };
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
                object result = new DataTable().Compute(ParseVariable(text), "");
                return result == DBNull.Value ? int.TryParse(text, out var i) ? i : 0 : int.TryParse(result.ToString(), out var ii) ? ii : 0;
            }
            string ParseVariable(string text)
            {
                return text
                    .Replace("{player.x}", Player?.TileX)
                    .Replace("{player.y}", Player?.TileY)
                    .Replace("{player.name}", Player?.Name)
                    .Replace("{player.heal}", Player?.TPlayer.statLife)
                    .Replace("{player.life}", Player?.TPlayer.statLife)
                    .Replace("{player.mana}", Player?.TPlayer.statMana)
                    .Replace("{player.magic}", Player?.TPlayer.statMana)
                    ;
            }
        }
    }
}
