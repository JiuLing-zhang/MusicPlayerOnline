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
    public class MiGuBuilder : BuildHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public MiGuBuilder(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new MiGuMusicProvider();
        }

        public override async Task<MusicDetail2> DoBuild(MusicSearchResult music)
        {
            return await _myMusicProvider.GetMusicDetail(music);
        }
    }
}
