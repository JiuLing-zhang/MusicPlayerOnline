using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public interface IMusicProvider
    {
        Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult> musics)> Search(string keyword);
        Task<MusicDetail2> GetMusicDetail(MusicSearchResult sourceMusic);
    }
}
