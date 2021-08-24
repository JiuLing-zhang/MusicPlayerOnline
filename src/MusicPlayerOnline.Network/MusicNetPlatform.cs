using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.BuildMusicDetail;
using MusicPlayerOnline.Network.SearchMusic;

namespace MusicPlayerOnline.Network
{
    public class MusicNetPlatform
    {
        //搜索链
        private readonly SearchHandler _neteaseSearcher = new NeteaseSearcher(PlatformEnum.Netease);
        private readonly SearchHandler _kuGouSearcher = new KuGouSearcher(PlatformEnum.KuGou);

        //构建详情链
        private readonly BuildHandler _neteaseBuilder = new NeteaseBuilder(PlatformEnum.Netease);
        private readonly BuildHandler _kuGouBuilder = new KuGouBuilder(PlatformEnum.KuGou);
        public MusicNetPlatform()
        {
            //搜索
            _neteaseSearcher.SetNextHandler(_kuGouSearcher);
            //详情
            _neteaseBuilder.SetNextHandler(_kuGouBuilder);
        }

        public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
        {
            return await _neteaseSearcher.Search(platform, keyword);
        }

        public async Task<MusicDetail2> BuildMusicDetail(MusicSearchResult music)
        {
            return await _neteaseBuilder.Build(music);
        }
    }
}
