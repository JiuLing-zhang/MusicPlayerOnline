using System;
using System.IO;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Common
{
    public class GlobalArgs
    {
        public static string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        public static string AppDataPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string AppDbFileName = Path.Combine(AppDataPath, "data.db3");
        public static string AppMusicCachePath = Path.Combine(AppDataPath, "Music");

        public static Config AppConfig;
    }
}
