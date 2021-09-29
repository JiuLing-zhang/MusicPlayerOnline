using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network;

namespace MusicPlayerOnline.Service
{
    public class SearchService : ISearchService
    {
        private readonly MusicNetPlatform _musicNetPlatform = new MusicNetPlatform();
        public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
        {
            return await _musicNetPlatform.Search(platform, keyword);
        }

        public async Task SaveResultToPlaylist(MusicSearchResult music)
        {
            var musicDetail = await _musicNetPlatform.BuildMusicDetail(music);
            if (musicDetail == null)
            {
                //TODO 通知
                //DependencyService.Get<IToast>().Show("该歌曲的信息似乎没找到~~~");
                return;
            }
        }
    }
}
