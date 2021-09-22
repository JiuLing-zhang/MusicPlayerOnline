using System;
using System.IO;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalArgs
    {
        private static string AppDirectory = "MusicPlayerOnline";
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDirectory);
        public static string AppDbPath = Path.Combine(AppDataPath, "data.db3");
    }
}
