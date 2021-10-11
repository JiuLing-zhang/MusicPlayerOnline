using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Services
{
    public class PlayerService
    {
        private static IAudio _audio;

        private static readonly PlayerService MyPlayerService = new PlayerService();
        public static PlayerService Instance()
        {
            return MyPlayerService;
        }
        public PlayerService()
        {
            _audio = DependencyService.Get<IAudio>();
            _audio.MediaBegin += Audio_MediaBegin;
            _audio.MediaEnded += Audio_MediaEnded;
            _audio.MediaFailed += Audio_MediaFailed;
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying => _audio.IsPlaying;
        /// <summary>
        /// 正在播放的歌曲信息
        /// </summary>
        public MusicDetail PlayingMusic;

        private void Audio_MediaBegin()
        {

        }

        private void Audio_MediaEnded()
        {
        }
        private void Audio_MediaFailed()
        {
            DependencyService.Get<IToast>().Show("播放失败");
        }

        public void Play(MusicDetail music)
        {
            _audio.Play(music.CachePath);
            PlayingMusic = music;
        }

        public void Start()
        {
            _audio.Start();
        }
        public void Pause()
        {
            _audio.Pause();
        }

        public (int Duration, int Position) GetPosition()
        {
            return _audio.GetPosition();
        }
    }
}
