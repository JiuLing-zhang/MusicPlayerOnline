using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class PlaylistService : IPlaylistService
    {
        public async Task Clear()
        {
            await DatabaseProvide.DatabaseAsync.DeleteAllAsync<Playlist>();
        }

        public async Task<List<Playlist>> GetList()
        {
            return await DatabaseProvide.DatabaseAsync.Table<Playlist>().ToListAsync();
        }

        public async Task Add(MusicDetail music)
        {
            if (await DatabaseProvide.DatabaseAsync.Table<Playlist>().Where(x => x.MusicDetailId == music.Id).CountAsync() == 0)
            {
                await DatabaseProvide.DatabaseAsync.InsertAsync(new Playlist() { MusicDetailId = music.Id, Name = music.Name, Artist = music.Artist });
            }
        }

        public async Task Delete(string musicDetailId)
        {
            await DatabaseProvide.DatabaseAsync.DeleteAsync<Playlist>(musicDetailId);
        }
    }
}
