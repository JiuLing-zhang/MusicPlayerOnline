using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class PlaylistService : IPlaylistService
    {
        public async Task<List<Playlist>> GetList()
        {
            return await DatabaseProvide.Database.Table<Playlist>().ToListAsync();
        }

        public async Task Add(MusicDetail music)
        {
            if (await DatabaseProvide.Database.Table<Playlist>().Where(x => x.MusicDetailId == music.Id).CountAsync() == 0)
            {
                await DatabaseProvide.Database.InsertAsync(new Playlist() { MusicDetailId = music.Id, Name = music.Name, Artist = music.Artist });
            }
        }
    }
}
