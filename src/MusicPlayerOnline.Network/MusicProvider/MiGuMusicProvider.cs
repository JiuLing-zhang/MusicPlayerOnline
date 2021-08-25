using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.Utils;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public class MiGuMusicProvider : IMusicProvider
    {
        private readonly HttpClient _httpClient = new();
        private const PlatformEnum Platform = PlatformEnum.MiGu;

        public MiGuMusicProvider()
        {
            _httpClient.DefaultRequestHeaders.Add("Host", "www.migu.cn");
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Mobile Safari/537.36 Edg/92.0.902.78");

        }
        public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult> musics)> Search(string keyword)
        {
            string args = MiGuUtils.GetSearchData(keyword);
            string url = $"{UrlBase.MiGu.Search}?{args}";
            string html = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            html = MiGuUtils.GetSearchResult(html);
            return (false, "", null);
        }

        public Task<MusicDetail2> GetMusicDetail(MusicSearchResult sourceMusic)
        {
            throw new NotImplementedException();
        }
    }
}
