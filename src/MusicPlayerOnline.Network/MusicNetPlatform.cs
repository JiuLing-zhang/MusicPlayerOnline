using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.BuildMusicDetail;
using MusicPlayerOnline.Network.MusicProvider;
using MusicPlayerOnline.Network.SearchMusic;

namespace MusicPlayerOnline.Network
{
    public class MusicNetPlatform
    {
        //搜索链
        private readonly SearchHandler _neteaseSearcher = new NeteaseSearcher(PlatformEnum.Netease);
        private readonly SearchHandler _kuGouSearcher = new KuGouSearcher(PlatformEnum.KuGou);
        private readonly SearchHandler _miGuSearcher = new MiGuSearcher(PlatformEnum.MiGu);

        //构建详情链
        private readonly BuildHandler _neteaseBuilder = new NeteaseBuilder(PlatformEnum.Netease);
        private readonly BuildHandler _kuGouBuilder = new KuGouBuilder(PlatformEnum.KuGou);
        private readonly BuildHandler _miGuBuilder = new MiGuBuilder(PlatformEnum.MiGu);
        
        public MusicNetPlatform()
        {
            //搜索
            _neteaseSearcher.SetNextHandler(_kuGouSearcher);
            _kuGouSearcher.SetNextHandler(_miGuSearcher);
            //详情
            _neteaseBuilder.SetNextHandler(_kuGouBuilder);
            _kuGouBuilder.SetNextHandler(_miGuBuilder);
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
