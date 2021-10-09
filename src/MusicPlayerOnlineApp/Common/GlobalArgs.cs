using System;
using System.IO;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Player;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalArgs
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        public static string AppDbFileName = Path.Combine(AppDataPath, "data.db3");
        public static string AppMusicCachePath = Path.Combine(AppDataPath, "Music");
        public static IAudio Audio;
        public static Config AppConfig;
    }

    /// <summary>
    /// 内部订阅时使用的关键字
    /// </summary>
    public class SubscribeKey
    {
        /// <summary>
        /// 播放
        /// </summary>
        public const string Play = "Play";
        /// <summary>
        /// 歌曲搜索完成
        /// </summary>
        public const string SearchFinished = "SearchFinished";
    }
}
