using System.IO;
using Newtonsoft.Json;

namespace TMenu
{
    internal class Config
    {
        static Config _instance;
        public static Config Instance
        {
            get
            {
                _instance ??= Load();
                return _instance;
            }
        }
        private static Config Load()
        {
            if (!File.Exists(Core.FileManager.ConfigFilePath))
                File.WriteAllText(Core.FileManager.ConfigFilePath, JsonConvert.SerializeObject(new()));
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(Core.FileManager.ConfigFilePath));
        }

        public string xx { get; set; }
    }
}
