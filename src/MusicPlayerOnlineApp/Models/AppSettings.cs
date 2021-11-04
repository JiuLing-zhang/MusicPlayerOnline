using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlayerOnlineApp.Models
{
    public class AppSettings
    {
        public string Database { get; set; }
        public string ApiAddress { get; set; }

        public string FileCachePath { get; set; }
    }
}
