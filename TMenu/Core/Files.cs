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

        public static void Init()
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
                if(Json.Deserilize(file) is { } panel)
                {
                    Data.Menus.Add(panel);
                    TShock.Log.Info($"[TMenu] Successfully loaded menu: \"{panel.Name}\"");
                }
            });
        }
    }
}
