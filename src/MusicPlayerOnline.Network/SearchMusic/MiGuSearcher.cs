using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.SearchMusic
{
    public class MiGuSearcher : SearchHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public MiGuSearcher(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new MiGuMusicProvider();
        }

        public override async Task DoSearch(string keyword, Action<List<MusicSearchResult>> searchCallback)
        {
            var result = await _myMusicProvider.Search(keyword);
            if (result.IsSucceed == false)
            {
                await Logger.WriteAsync(LogTypeEnum.警告, $"搜索咪咕歌曲失败：{result.ErrMsg}");
                return;
            }
            searchCallback(result.musics);
        }
    }
}
