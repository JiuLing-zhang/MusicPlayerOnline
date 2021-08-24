using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    result.AddRange(musics.Select(music => new MusicSearchResult()
                    {
                        Id = Guid.NewGuid().ToString("N"),
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
                    }));
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
