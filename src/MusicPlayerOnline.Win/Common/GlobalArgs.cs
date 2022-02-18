using System;
using System.IO;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Win.Common
{
    public class GlobalArgs
    {
        /// <summary>
        /// App名称
        /// </summary>
        public static string AppName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        public static string AppDbFileName = Path.Combine(AppDataPath, AppName, "data.db3");
        public static string AppMusicCachePath = Path.Combine(AppDataPath, AppName, "Music");

        public static Config AppConfig;
    }
}
