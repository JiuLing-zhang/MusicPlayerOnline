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
        public static void Init()
        {
            _audio= DependencyService.Get<IAudio>();
            _audio.MediaBegin += Audio_MediaBegin;
            _audio.MediaEnded += Audio_MediaEnded;
            _audio.MediaFailed += Audio_MediaFailed;
        }

        private static void Audio_MediaBegin()
        {

        }

        private static void Audio_MediaEnded()
        {
        }
        private static void Audio_MediaFailed()
        {
            DependencyService.Get<IToast>().Show("播放失败");
        }

        public static void Play(MusicDetail music)
        {
            _audio.Play(music.CachePath);
            GlobalArgs.PlayingMusic = music;
        }

        public static void Start()
        {
            _audio.Start();
        }
        public static void Pause()
        {
            _audio.Pause();
        }
    }
}
