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
        
        public static void Init()
        {
            Config._instance = null;
        }
    }
}
