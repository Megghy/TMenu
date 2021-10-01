using System.IO;
using Newtonsoft.Json;

namespace TMenu
{
    internal class Config
    {
        internal static Config _instance;
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
            if (!File.Exists(Core.IO.ConfigFilePath))
                File.WriteAllText(Core.IO.ConfigFilePath, JsonConvert.SerializeObject(new()));
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(Core.IO.ConfigFilePath));
        }

        public string xx { get; set; }
    }
}
