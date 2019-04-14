using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace R6SAdapter
{
    static class Config
    {
        public static Configuration Configuration;
        public static void SaveConfiguration()
        {

            string configPath = Path.Combine(Environment.CurrentDirectory,"Config");
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
                Config.Configuration = new Configuration();
            }
            string configString = JsonConvert.SerializeObject(Config.Configuration);
            File.WriteAllText(Path.Combine(configPath,"Config.txt"), configString);
        }
        public static void LoadConfiguration()
        {
            string configFilePath = Path.Combine(Environment.CurrentDirectory, "Config","Config.txt");
            if (!File.Exists(configFilePath)) SaveConfiguration();
            string configString = File.ReadAllText(configFilePath);
            Config.Configuration = JsonConvert.DeserializeObject<Configuration>(configString);
        }
    }
    class Configuration
    {
        public string R6SPath = string.Empty;
    }
}
