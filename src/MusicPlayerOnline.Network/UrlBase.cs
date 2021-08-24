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

        public class KuGou
        {
            public const string Search = "https://complexsearch.kugou.com/v2/search/song";
            public const string GetMusicUrl = "https://wwwapi.kugou.com/yy/index.php";
        }
    }
}
