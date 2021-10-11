using System;
using System.IO;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalArgs
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        public static string AppDbFileName = Path.Combine(AppDataPath, "data.db3");
        public static string AppMusicCachePath = Path.Combine(AppDataPath, "Music");
        public static Config AppConfig;
    }

    /// <summary>
    /// 内部订阅时使用的关键字
    /// </summary>
    public class SubscribeKey
    {
        /// <summary>
        /// 更新播放列表
        /// </summary>
        public const string UpdatePlaylist = "UpdatePlaylist";
    }
}
