using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Network.SearchMusic
{
    public abstract class SearchHandler
    {
        private SearchHandler _nextHandler;
        private readonly PlatformEnum _platform;
        protected SearchHandler(PlatformEnum platform)
        {
            _platform = platform;
        }
        public void SetNextHandler(SearchHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
        {
            var result = new List<MusicSearchResult>();
            if ((platform & _platform) == _platform)
            {
                await DoSearch(keyword, (musics) =>
                {
                    foreach (var music in musics)
                    {
                        string id = "";
                        if (music.Id.IsEmpty())
                        {
                            id = $"{music.Platform.GetDescription()}-{music.Name}";
                        }
                        else
                        {
                            id = $"{music.Platform.GetDescription()}-{music.PlatformId}";
                        }
                        result.Add(new MusicSearchResult()
                        {
                            Id = id,
                            Platform = music.Platform,
                            PlatformId = music.PlatformId,
                            Name = music.Name,
                            Alias = music.Alias == "" ? "" : $"（{music.Alias}）",
                            Artist = music.Artist,
                            ImageUrl = music.ImageUrl,
                            Album = music.Album,
                            Duration = music.Duration,
                            DurationText = music.DurationText,
                            PlayUrl = music.PlayUrl,
                            PlatformData = music.PlatformData
                        });
                    }
                });
            }
            if (_nextHandler != null)
            {
                result.AddRange(await _nextHandler.Search(platform, keyword));
            }
            return result;
        }
        public abstract Task DoSearch(string keyword, Action<List<MusicSearchResult>> searchCallback);
    }
}
