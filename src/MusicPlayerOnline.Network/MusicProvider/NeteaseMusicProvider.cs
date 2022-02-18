using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.Netease;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public class NeteaseMusicProvider : IMusicProvider
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const PlatformEnum Platform = PlatformEnum.Netease;

        public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult> musics)> Search(string keyword)
        {
            string url = $"{UrlBase.Netease.Search}";

            var postData = NeteaseUtils.GetPostDataForSearch(keyword);
            var form = new FormUrlEncodedContent(postData);
            var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ResultBase<MusicSearchHttpResult> result;
            try
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBase<MusicSearchHttpResult>>(json);
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(LogTypeEnum.错误, $"解析网易搜索结果失败：{ex.Message}.{ex.StackTrace}");
                return (false, "解析数据失败", null);
            }

            if (result == null)
            {
                return (false, "请求服务器失败", null);
            }
            if (result.code != 200)
            {
                return (false, result.msg, null);
            }

            var musics = new List<MusicSearchResult>();
            if (result.result.songCount == 0)
            {
                return (true, "", new List<MusicSearchResult>());
            }
            foreach (var song in result.result.songs)
            {
                try
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
                    var music = new MusicSearchResult()
                    {
                        Platform = Platform,
                        PlatformId = song.id.ToString(),
                        Name = song.name,
                        Alias = alia,
                        Artist = artistName,
                        Album = song.al.name,
                        ImageUrl = song.al.picUrl,
                        Duration = song.dt,
                        DurationText = $"{ts.Minutes}:{ts.Seconds:D2}",
                        PlatformData = new SearchResultExtended()
                        {
                            Fee = song.fee
                        }
                    };
                    musics.Add(music);
                }
                catch (Exception ex)
                {
                    await Logger.WriteAsync(LogTypeEnum.错误, $"构建网易搜索结果失败：{ex.Message}.{ex.StackTrace}");
                }

            }
            return (true, "", musics);
        }

        public async Task<MusicDetail> GetMusicDetail(MusicSearchResult sourceMusic)
        {
            string url = $"{UrlBase.Netease.GetMusic}";
            var postData = NeteaseUtils.GetPostDataForMusicUrl(sourceMusic.PlatformId);

            var form = new FormUrlEncodedContent(postData);
            var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var httpResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBase<MusicUrlHttpResult>>(json);
            if (httpResult == null)
            {
                return null;
            }
            if (httpResult.code != 200)
            {
                return null;
            }

            if (httpResult.data.Count == 0)
            {
                return null;
            }

            string playUrl = httpResult.data[0].url;
            if (playUrl.IsEmpty())
            {
                return null;
            }

            //获取歌词
            url = $"{UrlBase.Netease.Lyric}";
            postData = NeteaseUtils.GetPostDataForLyric(sourceMusic.PlatformId);

            form = new FormUrlEncodedContent(postData);
            response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
            json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var lyricResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MusicLyricHttpResult>(json);
            if (lyricResult == null)
            {
                return null;
            }
            if (lyricResult.code != 200)
            {
                return null;
            }

            if (lyricResult.lrc == null)
            {
                return null;
            }

            return new MusicDetail()
            {
                Id = sourceMusic.Id,
                Platform = sourceMusic.Platform,
                PlatformId = sourceMusic.PlatformId,
                Name = sourceMusic.Name,
                Alias = sourceMusic.Alias,
                Artist = sourceMusic.Artist,
                Album = sourceMusic.Album,
                Duration = sourceMusic.Duration,
                ImageUrl = sourceMusic.ImageUrl,
                PlayUrl = playUrl,
                Lyric = lyricResult.lrc.lyric
            };
        }

        public async Task<MusicDetail> UpdateMusicDetail(MusicDetail music)
        {
            string url = $"{UrlBase.Netease.GetMusic}";
            var postData = NeteaseUtils.GetPostDataForMusicUrl(music.PlatformId);

            var form = new FormUrlEncodedContent(postData);
            var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var httpResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBase<MusicUrlHttpResult>>(json);
            if (httpResult == null)
            {
                return music;
            }
            if (httpResult.code != 200)
            {
                return music;
            }

            if (httpResult.data.Count == 0)
            {
                return music;
            }

            string playUrl = httpResult.data[0].url;
            if (playUrl.IsEmpty())
            {
                return music;
            }

            music.PlayUrl = playUrl;
            return music;
        }
    }
}
