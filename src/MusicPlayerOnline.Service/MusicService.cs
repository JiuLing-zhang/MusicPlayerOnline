using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.Net;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network;

namespace MusicPlayerOnline.Service
{
    public class MusicService : IMusicService
    {
        private readonly MusicNetPlatform _musicNetPlatform = new MusicNetPlatform();
        private readonly HttpClientHelper _httpClient = new HttpClientHelper();
        public async Task<MusicDetail> GetMusicDetail(string id)
        {
            return await DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(MusicDetail music)
        {
            if (await DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == music.Id).CountAsync() == 0)
            {
                await DatabaseProvide.Database.InsertAsync(music);
            }
        }

        public async Task CacheMusic(MusicDetail music, string cachePath)
        {
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(cachePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(cachePath));
            }
            music = await _musicNetPlatform.UpdateMusicDetail(music);
            var data = await _httpClient.GetReadByteArray(music.PlayUrl);
            System.IO.File.WriteAllBytes(cachePath, data);
        }
    }
}
