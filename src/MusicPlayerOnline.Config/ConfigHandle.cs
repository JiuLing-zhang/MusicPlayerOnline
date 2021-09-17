using System;
using System.IO;

namespace MusicPlayerOnline.Config
{
    internal static class ConfigHandle
    {
        private static readonly string ConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";
        private static bool _isReadingConfig = false;
        internal static Config Read()
        {
            _isReadingConfig = true;
            if (!File.Exists(ConfigPath))
            {
                throw new FileNotFoundException("配置文件未找到");
            }
            string json = File.ReadAllText(ConfigPath);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(json);
            _isReadingConfig = false;
            return obj;
        }
        internal static void Save()
        {
            if (_isReadingConfig)
            {
                return;
            }
            if (!File.Exists(ConfigPath))
            {
                throw new FileNotFoundException("配置文件未找到");
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(AppSetting.Setting);
            File.WriteAllText(ConfigPath, json);
        }
    }
}
