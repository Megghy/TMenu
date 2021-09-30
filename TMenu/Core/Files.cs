using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using TerrariaUI.Base;
using TerrariaUI.Widgets;
using TMenu.Controls;
using TShockAPI;
using static Terraria.WorldBuilding.Searches;

namespace TMenu.Core
{
    internal class Files
    {
        public static string SavePath => Path.Combine(TShock.SavePath, "TMenu");
        public static string ConfigFilePath => Path.Combine(SavePath, "TMenuConfig.json");
        public static string MenuFilePath => Path.Combine(SavePath, "Menus");

        public static void Load()
        {
            Config._instance = null;
            Data.Menus.ForEach(m => m.Dispose());
            Data.Menus.Clear();
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            if (!Directory.Exists(MenuFilePath))
                Directory.CreateDirectory(MenuFilePath);
            Directory.GetFiles(MenuFilePath).ForEach(file =>
            {
                if(Parser.Deserilize(file) is { } panel)
                {
                    Data.Menus.Add(panel);
                    //TShock.Log.ConsoleInfo($"[TMenu] Successfully loaded menu: \"{panel.Name}\".");
                    Logs.Success($"成功加载菜单: \"{panel.Name}\"");
                }
            });
        }
        public static TPanel FindMenu(string name)
        {
            return Data.Menus.FirstOrDefault(m => m.Name == name);
        }
        public static List<TPanel> FindMenuLike(string name)
        {
            return Data.Menus.Where(m => m.Name.ToLower().StartsWith(name.ToLower()) || m.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
