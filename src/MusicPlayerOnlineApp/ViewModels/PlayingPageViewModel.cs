using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Services;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class PlayingPageViewModel : ViewModelBase
    {
        private readonly IMusicService _musicService;
        public Command PlayerStateChangeCommand => new Command(PlayerStateChange);
        public PlayingPageViewModel()
        {
            _musicService = new MusicService();
            CurrentMusic = GlobalArgs.PlayingMusic;
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "正在播放";

        private bool _isPlaying;
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        private MusicDetail _currentMusic;
        /// <summary>
        /// 当前播放的歌曲
        /// </summary>
        public MusicDetail CurrentMusic
        {
            get => _currentMusic;
            set
            {
                _currentMusic = value;
                OnPropertyChanged();
            }
        }


        private async void PlayerStateChange()
        {
            if (IsPlaying == true)
            {
                PlayerService.Pause();
            }
            else
            {
                PlayerService.Start();
            }

            IsPlaying = !IsPlaying;
        }
    }
}
