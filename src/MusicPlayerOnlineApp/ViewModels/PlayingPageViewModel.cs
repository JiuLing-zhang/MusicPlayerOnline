using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class PlayingPageViewModel : ViewModelBase
    {
        public Command PlayerStateChangeCommand => new Command(PlayerStateChange);
        public PlayingPageViewModel()
        {
            Common.GlobalArgs.Audio.MediaBegin += Audio_MediaBegin;
            Common.GlobalArgs.Audio.MediaEnded += Audio_MediaEnded;
            Common.GlobalArgs.Audio.MediaFailed += Audio_MediaFailed;
        }

        public async void OnAppearing()
        {
            CurrentMusic = GlobalArgs.CurrentMusic;
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

        private void Audio_MediaBegin()
        {
            CurrentMusic = GlobalArgs.CurrentMusic;
            IsPlaying = true;
        }

        private void Audio_MediaEnded()
        {
            IsPlaying = false;
        }
        private void Audio_MediaFailed()
        {
            //TODO 提示
            // DependencyService.Get<IToast>().Show("播放失败");
        }

        private async void PlayerStateChange()
        {
            if (IsPlaying == true)
            {
                Common.GlobalArgs.Audio.Pause();
            }
            else
            {
                Common.GlobalArgs.Audio.Start();
            }

            IsPlaying = !IsPlaying;
        }
    }
}
