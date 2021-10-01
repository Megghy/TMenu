using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using TerrariaUI;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TMenu.Controls;
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
                return new();
            var s = new T();
            s.Stratify(style);
            return s;
        }
        public static string Replace(this string s, string oldValue, object newValue) => s.Replace(oldValue, newValue?.ToString());
        /// <summary>
        /// 如果是从文件加载的话返回重新加载的菜单对象
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="plr"></param>
        /// <returns></returns>
        public static TPanel Show(this TPanel panel, TSPlayer plr = null, bool clone = true)
        {
            var tempPanel = panel;
            if (panel.IsFromFile && clone)
                tempPanel = panel.Clone(plr) as TPanel;
            if (tempPanel.Personal && plr != null)
                tempPanel.AddUser(plr);
            tempPanel.TUIObject = TUI.Create(tempPanel.TUIObject);
            return tempPanel;
        }
        public static TSPlayer Player(this Touch t) => TShock.Players[t.PlayerIndex];
        public static List<TPanel> MenuList(this TSPlayer plr) => plr.GetData<List<TPanel>>("TMenu.Using");
        public static bool ContainsMenu(this TSPlayer plr, string name) => plr.MenuList().Any(m => m.Name == name);
        public static bool ContainsMenu(this TSPlayer plr, TPanel panel) => plr.MenuList().Any(m => m.Name == panel.Name);
        /// <summary>
        /// 创建指定菜单(的副本)并打开
        /// </summary>
        /// <param name="plr"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static bool OpenMenu(this TSPlayer plr, TPanel panel, bool clone = true)
        {
            if (!plr.ContainsMenu(panel))
            {
                plr?.MenuList().Add(panel.Show(plr, clone));
                Logs.Info($"玩家 {plr.Name} {"开启".Color("C3DB9E")} 菜单: \"{panel.Name}\"");
                return true;
            }
            else
                Logs.Warn($"玩家 {plr.Name} 已开启过菜单 {panel.Name}");
            return false;
        }
        public static bool OpenMenu(this TSPlayer plr, Data.MenuOriginData data, bool clone = false) => OpenMenu(plr, Core.Parser.Deserilize(data, plr), clone);
        public static bool CloseMenu(this TSPlayer plr, string name, bool log = true)
            => plr.MenuList().FirstOrDefault(m => m.Name == name) is { } menu && plr.CloseMenu(menu, log);
        public static bool CloseMenu(this TSPlayer plr, TPanel panel, bool log = true)
        {
            if (plr.ContainsMenu(panel))
            {
                panel.Dispose();
                plr.MenuList().Remove(panel);
                if (log)
                    Logs.Info($"玩家 {plr.Name} {"关闭".Color("DBA79E")} 菜单: \"{panel.Name}\"");
                return true;
            }
            else
                return false;
        }
        public static void CloseAllMenu(this TSPlayer plr, bool log = true) => plr?.MenuList().ToArray().ToList().ForEach(m => plr.CloseMenu(m, log));
        public static T CreateClone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
