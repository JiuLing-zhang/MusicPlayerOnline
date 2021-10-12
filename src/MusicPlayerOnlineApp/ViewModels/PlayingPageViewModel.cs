using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class PlayingPageViewModel : ViewModelBase
    {
        private readonly IMusicService _musicService;
        public Command PlayerStateChangeCommand => new Command(PlayerStateChange);
        public Command RepeatTypeChangeCommand => new Command(RepeatTypeChange);
        public Action<LyricDetailViewModel> ScrollLyric { get; set; }
        public PlayingPageViewModel()
        {
            Lyrics = new ObservableCollection<LyricDetailViewModel>();
            _musicService = new MusicService();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    Device.BeginInvokeOnMainThread(UpdatePlayingProgress);
                    return true;
                }
           );
        }

        public void OnAppearing()
        {
            UpdatePlayingMusicInfo();
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

        private ObservableCollection<LyricDetailViewModel> _lyrics;
        /// <summary>
        /// 每行的歌词
        /// </summary>
        public ObservableCollection<LyricDetailViewModel> Lyrics
        {
            get => _lyrics;
            set
            {
                _lyrics = value;
                OnPropertyChanged();
            }
        }

        private decimal _position;
        /// <summary>
        /// 播放进度
        /// </summary>
        public decimal Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }


        private string _durationTime;
        /// <summary>
        /// 时长时间
        /// </summary>
        public string DurationTime
        {
            get => _durationTime;
            set
            {
                _durationTime = value;
                OnPropertyChanged();
            }
        }

        private string _currentTime;
        /// <summary>
        /// 当前时间
        /// </summary>
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private int _playModeInt;
        /// <summary>
        /// 播放模式
        /// </summary>
        public int PlayModeInt
        {
            get => _playModeInt;
            set
            {
                _playModeInt = value;
                OnPropertyChanged();
            }
        }

        private void UpdatePlayingMusicInfo()
        {
            if (PlayerService.Instance().PlayingMusic == null)
            {
                return;
            }

            if (CurrentMusic?.Id == PlayerService.Instance().PlayingMusic.Id)
            {
                return;
            }
            CurrentMusic = PlayerService.Instance().PlayingMusic;
            GetLyricDetail();
        }
        /// <summary>
        /// 解析歌词
        /// </summary>
        private void GetLyricDetail()
        {
            if (CurrentMusic.Lyric.IsEmpty())
            {
                return;
            }

            string pattern = ".*";
            var lyricRowList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(CurrentMusic.Lyric, pattern);
            var lyricGroupNames = new List<string>() { "mm", "ss", "fff", "lyric" };
            foreach (var lyricRow in lyricRowList)
            {
                if (lyricRow.IsEmpty())
                {
                    continue;
                }
                pattern = @"\[(?<mm>\d*):(?<ss>\d*).(?<fff>\d*)\](?<lyric>.*)";
                var (success, result) = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(lyricRow, pattern, lyricGroupNames);
                if (success == false)
                {
                    continue;
                }

                int totalMillisecond = Convert.ToInt32(result.mm) * 60 * 1000 + Convert.ToInt32(result.ss) * 1000 + Convert.ToInt32(result.fff);
                var info = result.lyric;
                Lyrics.Add(new LyricDetailViewModel() { Position = totalMillisecond, Info = info });
            }
        }

        /// <summary>
        /// 更新播放进度
        /// </summary>
        private void UpdatePlayingProgress()
        {
            if (PlayerService.Instance().PlayingMusic == null)
            {
                return;
            }

            if (PlayerService.Instance().IsPlaying == false)
            {
                return;
            }

            var (duration, position) = PlayerService.Instance().GetPosition();
            Position = (decimal)position / duration;

            var tsDurationTime = TimeSpan.FromMilliseconds(duration);
            DurationTime = $"{tsDurationTime.Minutes:D2}:{tsDurationTime.Seconds:D2}";

            var tsCurrentTime = TimeSpan.FromMilliseconds(position);
            CurrentTime = $"{tsCurrentTime.Minutes:D2}:{tsCurrentTime.Seconds:D2}";

            IsPlaying = PlayerService.Instance().IsPlaying;

            //取大于当前进度的第一行索引，在此基础上-1则为需要高亮的行
            int highlightIndex = 0;
            foreach (var lyric in Lyrics)
            {
                lyric.IsHighlight = false;
                if (lyric.Position > position)
                {
                    break;
                }
                highlightIndex++;
            }
            if (highlightIndex > 0)
            {
                highlightIndex = highlightIndex - 1;
            }

            Lyrics[highlightIndex].IsHighlight = true;
            ScrollLyric.Invoke(Lyrics[highlightIndex]);
        }
        //暂停、恢复
        private async void PlayerStateChange()
        {
            if (IsPlaying == true)
            {
                PlayerService.Instance().Pause();
            }
            else
            {
                PlayerService.Instance().Start();
            }

            IsPlaying = !IsPlaying;
        }
        //循环方式
        private void RepeatTypeChange()
        {
            switch (GlobalArgs.AppConfig.Player.PlayMode)
            {
                case PlayModeEnum.RepeatOne:
                    GlobalArgs.AppConfig.Player.PlayMode = PlayModeEnum.RepeatList;
                    break;
                case PlayModeEnum.RepeatList:
                    GlobalArgs.AppConfig.Player.PlayMode = PlayModeEnum.Shuffle;
                    break;
                case PlayModeEnum.Shuffle:
                    GlobalArgs.AppConfig.Player.PlayMode = PlayModeEnum.RepeatOne;
                    break;
            }
            PlayModeInt = (int)GlobalArgs.AppConfig.Player.PlayMode;
            DependencyService.Get<IToast>().Show($"{GlobalArgs.AppConfig.Player.PlayMode.GetDescription()}");
        }
    }
}
