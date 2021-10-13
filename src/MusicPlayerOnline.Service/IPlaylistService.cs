using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IPlaylistService
    {
        Task Clear();
        Task<List<Playlist>> GetList();
        Task Add(MusicDetail music);
        Task Delete(string musicDetailId);
    }
}
