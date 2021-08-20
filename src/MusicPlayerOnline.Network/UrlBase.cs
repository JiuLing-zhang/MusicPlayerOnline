using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Network
{
    public class UrlBase
    {
        public class Netease
        {
            public const string Search = "https://music.163.com/weapi/cloudsearch/get/web?csrf_token=";
            public const string GetMusicUrl = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";
        }
    }
}
