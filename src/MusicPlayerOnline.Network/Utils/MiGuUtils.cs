using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Network.Utils
{
    public class MiGuUtils
    {
        public static string GetSearchData(string keyword)
        {
            return $"migu_p=h5&pn=1&type=allLobby&_ch=&content={keyword}";
        }

        public static string GetSearchResult(string html)
        {
            string pattern = @"<ul class=""list\"">(?<data>[\S\s]*)other-keys";
            var result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);
            return "";
        }
    }
}
