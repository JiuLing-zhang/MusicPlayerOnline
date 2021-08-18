using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiuLing.CommonLibs.Model;
using JiuLing.CommonLibs.Net;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Netease;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public class NeteaseMusicProvider : IMusicProvider
    {
        private readonly HttpClientHelper _http = new HttpClientHelper();
        private const PlatformEnum Platform = PlatformEnum.Netease;

        public async Task<JsonResult<List<MusicInfo>>> Search(string keyword)
        {
            string url = $"{UrlBase.Netease.Search}{keyword}";

            var json = await _http.GetReadString(url);
            var result = System.Text.Json.JsonSerializer.Deserialize<ResultBase<MusicSearchResult>>(json);
            if (result == null)
            {
                return new JsonResult<List<MusicInfo>>() { Code = -1, Message = "请求服务器失败" };
            }
            if (result.code != 200)
            {
                return new JsonResult<List<MusicInfo>>() { Code = result.code, Message = result.msg };
            }

            var musics = new List<MusicInfo>();
            foreach (var song in result.result.songs)
            {
                var ts = TimeSpan.FromMilliseconds(song.duration);

                string alias = "";
                if (song.alias.Length > 0)
                {
                    alias = song.alias[0];
                }

                string artistName = "";
                if (song.artists.Length > 0)
                {
                    artistName = string.Join("、", song.artists.Select(x => x.name).ToList());
                }
                var music = new MusicInfo()
                {
                    Name = song.name,
                    Alias = alias,
                    ArtistName = artistName,
                    AlbumName = song.album.name,
                    Duration = song.duration,
                    DurationString = $"{ts.Minutes}:{ts.Seconds:D2}",
                    Platform = Platform
                };
                musics.Add(music);
            }
            return new JsonResult<List<MusicInfo>>() { Code = 0, Data = musics };
        }
    }
}
