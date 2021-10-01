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
        public static List<MenuOriginData> Menus { get; set; } = new();
        public static Dictionary<string, Type> ControlName { get; set; } = new();
        /// <summary>
        /// Func 参数: 调用者(TMenuControlBase派生类), 玩家对象, 返回数据
        /// </summary>
        public static Dictionary<string, Func<dynamic, TSPlayer, object>> VariableList { get; set; } = new();
        public static void Init()
        {
            ControlName.Clear();
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "TMenu.Controls" && t.BaseType.Name == "TMenuControlBase`1")
                .ForEach(t => ControlName.Add(t.GetCustomAttribute<NameInJsonAttribute>() is { } a ? a.Name : t.Name.Remove(0, 1).ToLower(), t));

            VariableList.Add("{player.x}", (sender, plr) => plr?.TileX);
            VariableList.Add("{player.y}", (sender, plr) => plr?.TileY);
            VariableList.Add("{player.name}", (sender, plr) => plr?.Name);
            VariableList.Add("{player.heal}", (sender, plr) => plr?.TPlayer.statLife);
            VariableList.Add("{player.life}", (sender, plr) => plr?.TPlayer.statLife);
            VariableList.Add("{player.mana}", (sender, plr) => plr?.TPlayer.statMana);
            VariableList.Add("{player.magic}", (sender, plr) => plr?.TPlayer.statMana);
        }
        public class Click
        {
            public string[] Command { get; set; }
            public string Goto { get; set; }
            public string Message { get; set; }
        }
        public class MenuOriginData
        {
            public MenuOriginData() : this("unknown", "0", "0", "0", "0")
            {
            }
            public MenuOriginData(string name, string x, string y, string width, string height, string text = null, UIConfiguration configuration = null, UIStyle style = null, Click clickCommand = null)
            {
                Name = name;
                OriginX = x;
                OriginY = y;
                OriginWidth = width;
                OriginHeight = height;
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
            public string Type { get; set; }
            public string OriginX { get; set; }
            public int X => ParsePosition(OriginX);
            public string OriginY { get; set; }
            public int Y => ParsePosition(OriginY);
            public string OriginWidth { get; set; }
            public int Width => ParsePosition(OriginWidth);
            public string OriginHeight { get; set; }
            public int Height => ParsePosition(OriginHeight);
            public string Text { get; set; }
            public UIConfiguration Config { get; set; }
            public UIStyle Style { get; set; }
            public Click ClickCommand { get; set; } = new();
            public List<MenuOriginData> Childs { get; set; } = new();

            #region 一些不一定存在的属性
            public Direction Direction { get; set; } = Direction.Up;
            public ItemData Item { get; set; } = new();
            public ItemData[] Items { get; set; } = new ItemData[40];
            public bool Personal { get; set; } = true;
            public bool Moveable { get; set; } = true;
            #endregion

            public TSPlayer Player;
            public dynamic Parent;
            public override string ToString()
            {
                return $"{Type} <{OriginX}, {OriginY}, {OriginWidth}, {OriginHeight}>";
            }
            public static bool TryParseFromJson(JToken json, out MenuOriginData data, TSPlayer plr = null)
            {
                try
                {
                    data = ParseFromJson(json, plr);
                    return true;
                }
                catch
                {
                    data = null;
                    return false;
                }
            }
            public static MenuOriginData ParseFromJson(JToken json, TSPlayer plr = null)
            {
                return new MenuOriginData(
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

                    Type = json.Value<string>("type"),
                    Item = json.Value<JObject>("item")?.ToObject<ItemData>(),
                    Items = json.Value<JArray>("items")?.ToObject<ItemData[]>(),
                    Direction = Enum.TryParse<Direction>(json.Value<JObject>("style")?.Value<string>("direction") ?? "", out var d) ? d : Direction.Up,
                    Moveable = json.Value<bool>("moveable"),
                    Personal = json.Value<bool>("personal"),
                    Player = plr
                };
            }
            public int ParsePosition(string text)
            {
                object result = new DataTable().Compute(ParseVariable(text, Parent, Player), "");
                return result == DBNull.Value ? int.TryParse(text, out var i) ? i : 0 : int.TryParse(result.ToString(), out var ii) ? ii : 0;
            }
            string ParseVariable(string text, dynamic sender, TSPlayer plr)
            {
                VariableList.ForEach(v => text = text.Replace(v.Key, v.Value(sender, plr).ToString()));
                return text;
            }
        }
    }
}
