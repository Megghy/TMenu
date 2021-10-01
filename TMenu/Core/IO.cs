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
    internal class IO
    {
        public static string SavePath => Path.Combine(TShock.SavePath, "TMenu");
        public static string ConfigFilePath => Path.Combine(SavePath, "TMenuConfig.json");
        public static string MenuFilePath => Path.Combine(SavePath, "Menus");

        public static void Load()
        {
            Config._instance = null;
            Data.Menus.Clear();
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            if (!Directory.Exists(MenuFilePath))
                Directory.CreateDirectory(MenuFilePath);
            Directory.GetFiles(MenuFilePath).ForEach(file =>
            {
                if(Parser.ReadOriginDataFromFile(file) is { } data)
                {
                    Data.Menus.Add(data);
                    Logs.Success($"已读取菜单: \"{data.Name}\"");
                }
            });
            void ClearData(Data.MenuOriginData data)
            {
                data.Parent = null;
                data.Childs.ForEach(c => ClearData(c));
            }
        }
        public static Data.MenuOriginData FindMenu(string name) => Data.Menus.FirstOrDefault(m => m.Name == name);
        public static List<Data.MenuOriginData> FindMenuLike(string name)
        {
            return Data.Menus.Where(m => m.Name.ToLower().StartsWith(name.ToLower()) || m.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
