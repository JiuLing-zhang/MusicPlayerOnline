﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.KuGou;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public class KuGouMusicProvider : IMusicProvider
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const PlatformEnum Platform = PlatformEnum.KuGou;
        public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult> musics)> Search(string keyword)
        {
            string args = KuGouUtils.GetSearchData(keyword);
            string url = $"{UrlBase.KuGou.Search}?{args}";

            string json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            json = KuGouUtils.RemoveHttpResultHead(json);
            if (json.IsEmpty())
            {
                return (false, "服务器响应异常", null);
            }
            var httpResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpResultBase<HttpMusicSearchResult>>(json);
            if (httpResult == null)
            {
                return (false, "请求服务器失败", null);
            }
            if (httpResult.status != 1 || httpResult.error_code != 0)
            {
                return (false, httpResult.error_msg, null);
            }

            var musics = new List<MusicSearchResult>();
            if (httpResult.data.lists.Length == 0)
            {
                return (true, "", new List<MusicSearchResult>());
            }
            foreach (var httpMusic in httpResult.data.lists)
            {
                try
                {
                    var ts = TimeSpan.FromSeconds(httpMusic.Duration);
                    var music = new MusicSearchResult()
                    {
                        Platform = Platform,
                        PlatformId = httpMusic.ID,
                        Name = KuGouUtils.RemoveSongNameTag(httpMusic.SongName),
                        Alias = "",
                        Artist = KuGouUtils.RemoveSongNameTag(httpMusic.SingerName),
                        Album = httpMusic.AlbumName,
                        Duration = (int)ts.TotalMilliseconds,
                        DurationText = $"{ts.Minutes}:{ts.Seconds:D2}",
                        PlatformData = new SearchResultExtended()
                        {
                            Hash = httpMusic.FileHash,
                            AlbumId = httpMusic.AlbumID
                        }
                    };
                    musics.Add(music);
                }
                catch (Exception ex)
                {
                    await Logger.WriteAsync(LogTypeEnum.错误, $"构建酷狗搜索结果失败：{ex.Message}.{ex.StackTrace}");
                }
            }
            return (true, "", musics);
        }

        public async Task<MusicDetail> GetMusicDetail(MusicSearchResult sourceMusic)
        {

            if (!(sourceMusic.PlatformData is SearchResultExtended platformData))
            {
                throw new ArgumentException("平台数据初始化异常");
            }
            string args = KuGouUtils.GetMusicUrlData(platformData.Hash, platformData.AlbumId);
            string url = $"{UrlBase.KuGou.GetMusic}?{args}";

            string json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            if (json.IsEmpty())
            {
                return null;
            }
            var httpResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpResultBase<MusicDetailHttpResult>>(json);
            if (httpResult == null)
            {
                return null;
            }
            if (httpResult.status != 1 || httpResult.error_code != 0)
            {
                return null;
            }

            args = KuGouUtils.GetMusicLyricData(platformData.Hash, platformData.AlbumId);
            url = $"{UrlBase.KuGou.Lyric}?{args}";
            json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);

            var lyricResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpResultBase<MusicLyricHttpResult>>(json);
            if (lyricResult == null)
            {
                return null;
            }
            if (lyricResult.status != 1 || lyricResult.error_code != 0)
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
                ImageUrl = httpResult.data.img,
                PlayUrl = httpResult.data.play_url,
                Lyric = lyricResult.data.lyrics
            };
        }

        public Task<MusicDetail> UpdateMusicDetail(MusicDetail music)
        {
            throw new NotImplementedException();
        }
    }
}
