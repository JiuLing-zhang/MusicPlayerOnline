using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IMusicService
    {
        Task<MusicDetail> GetMusicDetail(string id);
        Task Add(MusicDetail music);
        Task CacheMusic(MusicDetail music, string cachePath);
    }
}
