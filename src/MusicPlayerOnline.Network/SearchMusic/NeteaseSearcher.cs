using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.SearchMusic
{
    public class NeteaseSearcher : SearchHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public NeteaseSearcher(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new NeteaseMusicProvider();
        }

        public override async Task DoSearch(string keyword, Action<List<MusicSearchResult>> searchCallback)
        {
            var result = await _myMusicProvider.Search(keyword);
            if (result.IsSucceed == false)
            {
                //TODO 加入日志
                //Logger.Write($"搜索网易歌曲失败：{result.ErrMsg}");
                return;
            }

            searchCallback(result.musics);
        }
    }
}
