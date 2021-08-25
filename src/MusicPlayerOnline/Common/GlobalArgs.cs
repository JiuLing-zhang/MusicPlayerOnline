using System;

namespace MusicPlayerOnline.Common
{
    public class GlobalArgs
    {
        public static string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
    }
}
