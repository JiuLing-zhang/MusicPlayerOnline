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
            if (result.success == false)
            {
                return "";
            }

            pattern = @"<li class=""default"">[\S\s]*?</li>";
            var musicHtmlList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(result.result, pattern);
            if (musicHtmlList.Count == 0)
            {
                return "";
            }

            foreach (var musicHtml in musicHtmlList)
            {
                pattern = @"a href=""(?<url>\S*)""[\s\S]*<img src=""(?<imgUrl>\S*)""";
                result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(musicHtml, pattern);
            }
            return "";
        }
    }
}
