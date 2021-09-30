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

        public async Task<MusicDetail> GetMusicDetail(MusicSearchResult music)
        {
            return await _musicNetPlatform.BuildMusicDetail(music);
        }
    }
}
