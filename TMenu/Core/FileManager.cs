using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMenu.Core
{
    internal class FileManager
    {
        public static string SavePath => Path.Combine(TShockAPI.TShock.SavePath, "TMenu");
        public static string ConfigFilePath => Path.Combine(SavePath, "TMenuConfig.json");

        public static void Init()
        {
            Config._instance = null;

        }
    }
}
