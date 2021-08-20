using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JiuLing.CommonLibs.Model;
using JiuLing.CommonLibs.Net;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.Netease;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public class NeteaseMusicProvider : IMusicProvider
    {
        private readonly HttpClient _http = new HttpClient();
        private const PlatformEnum Platform = PlatformEnum.Netease;

        public async Task<JsonResult<List<MusicSearchResultModel>>> Search(string keyword)
        {
            string url = $"{UrlBase.Netease.Search}";

            var postData = NeteaseUtils.GetPostDataForSearch(keyword);
            var form = new FormUrlEncodedContent(postData);
            var response = await _http.PostAsync(url, form).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = System.Text.Json.JsonSerializer.Deserialize<ResultBase<MusicSearchHttpResult>>(json);
            if (result == null)
            {
                return new JsonResult<List<MusicSearchResultModel>>() { Code = -1, Message = "请求服务器失败" };
            }
            if (result.code != 200)
            {
                return new JsonResult<List<MusicSearchResultModel>>() { Code = result.code, Message = result.msg };
            }

            var musics = new List<MusicSearchResultModel>();
            if (result.result.songCount == 0)
            {
                return new JsonResult<List<MusicSearchResultModel>>() { Code = 0, Data = new List<MusicSearchResultModel>() };
            }
            foreach (var song in result.result.songs)
            {
                var ts = TimeSpan.FromMilliseconds(song.dt);

                string alia = "";
                if (song.alia.Length > 0)
                {
                    alia = song.alia[0];
                }

                string artistName = "";
                if (song.ar.Length > 0)
                {
                    artistName = string.Join("、", song.ar.Select(x => x.name).ToList());
                }
                var music = new MusicSearchResultModel()
                {
                    Platform = Platform,
                    Id = song.id,
                    Name = song.name,
                    Alias = alia,
                    ArtistName = artistName,
                    AlbumName = song.al.name,
                    Duration = song.dt,
                    DurationText = $"{ts.Minutes}:{ts.Seconds:D2}"

                };
                musics.Add(music);
            }
            return new JsonResult<List<MusicSearchResultModel>>() { Code = 0, Data = musics };
        }

        public async Task<JsonResult<string>> GetMusicUrl(int id)
        {
            string url = $"{UrlBase.Netease.GetMusicUrl}";

            var postData = NeteaseUtils.GetPostDataForMusicUrl(id);

            var form = new FormUrlEncodedContent(postData);
            var response = await _http.PostAsync(url, form).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = System.Text.Json.JsonSerializer.Deserialize<ResultBase<MusicUrlHttpResult>>(json);
            if (result == null)
            {
                return new JsonResult<string>() { Code = -1, Message = "获取歌曲真实地址失败" };
            }
            if (result.code != 200)
            {
                return new JsonResult<string>() { Code = result.code, Message = result.msg };
            }

            if (result.data.Count == 0)
            {
                return new JsonResult<string>() { Code = -1, Message = "服务器未返回歌曲真实地址" };
            }
            return new JsonResult<string>() { Code = 0, Data = result.data[0].url };
        }
    }
}
