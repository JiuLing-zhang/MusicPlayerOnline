using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail
{
    public class NeteaseBuilder : BuildHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public NeteaseBuilder(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new NeteaseMusicProvider();
        }
        public override async Task<MusicDetail> DoBuild(MusicSearchResult music)
        {
            return await _myMusicProvider.GetMusicDetail(music);
        }
    }
}
