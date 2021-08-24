using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.BuildMusicDetail
{
    public class KuGouBuilder : BuildHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public KuGouBuilder(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new KuGouMusicProvider();
        }

        public override async Task<MusicDetail2> DoBuild(MusicSearchResult music)
        {
            return await _myMusicProvider.GetMusicDetail(music);
        }
    }
}
