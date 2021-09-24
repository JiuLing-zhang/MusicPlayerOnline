using System;
using System.IO;
using MusicPlayerOnline.Player;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalArgs
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        public static string AppDbFileName = Path.Combine(AppDataPath, "data.db3");
        public static string AppMusicCachePath = Path.Combine(AppDataPath, "Music");
        public static IAudio Audio;
    }
}
