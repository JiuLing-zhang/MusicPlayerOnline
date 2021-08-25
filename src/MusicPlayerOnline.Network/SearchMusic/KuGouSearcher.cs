﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.SearchMusic
{
    public class KuGouSearcher : SearchHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public KuGouSearcher(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new KuGouMusicProvider();
        }

        public override async Task DoSearch(string keyword, Action<List<MusicSearchResult>> searchCallback)
        {
            var result = await _myMusicProvider.Search(keyword);
            if (result.IsSucceed == false)
            {
                //todo 记录日志  result.ErrMsg
                return;
            }
            searchCallback(result.musics);
        }
    }
}