﻿using MusicPlayerOnline.Model.Model;
using System;
using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Common;
using MusicPlayerOnline.App.Services;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.ViewModels
{
    public class PlayingPageViewModel : ViewModelBase
    {
        public Command PlayerStateChangeCommand => new Command(PlayerStateChange);
        public Command RepeatTypeChangeCommand => new Command(RepeatTypeChange);
        public Command PreviousCommand => new Command(Previous);
        public Command NextCommand => new Command(Next);

        public Command TempButtonCommand => new Command(() =>
        {
            DependencyService.Get<IToast>().Show("不知道放啥了，占个位而已~~");
        });

        public Action<LyricDetailViewModel> ScrollLyric { get; set; }
        public PlayingPageViewModel()
        {
            Lyrics = new ObservableCollection<LyricDetailViewModel>();
            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
                {
                    Device.BeginInvokeOnMainThread(UpdatePlayingProgress);
                    return true;
                }
           );

            PlayerService.Instance().MediaBegin += UpdatePlayingMusicInfo;

        }

        public void OnAppearing()
        {
            UpdatePlayingMusicInfo();
            PlayModeInt = (int)GlobalArgs.AppConfig.Player.PlayMode;
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
            Logger.Write(LogTypeEnum.消息, $"更新正在播放的歌曲，{CurrentMusic.Platform.GetDescription()},{CurrentMusic.Name}");
            GetLyricDetail();
        }
        /// <summary>
        /// 解析歌词
        /// </summary>
        private void GetLyricDetail()
        {
            Logger.Write(LogTypeEnum.消息, $"准备解析歌词");
            if (Lyrics.Count > 0)
            {
                Lyrics.Clear();
            }
            if (CurrentMusic.Lyric.IsEmpty())
            {
                Logger.Write(LogTypeEnum.消息, $"歌词不存在，跳过");
                return;
            }

            string pattern = ".*";
            var lyricRowList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(CurrentMusic.Lyric, pattern);
            foreach (var lyricRow in lyricRowList)
            {
                if (lyricRow.IsEmpty())
                {
                    continue;
                }
                pattern = @"\[(?<mm>\d*):(?<ss>\d*).(?<fff>\d*)\](?<lyric>.*)";
                var (success, result) = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(lyricRow, pattern);
                if (success == false)
                {
                    Logger.Write(LogTypeEnum.消息, $"播放页面解析歌词失败，{lyricRow}");
                    continue;
                }

                int totalMillisecond = Convert.ToInt32(result["mm"]) * 60 * 1000 + Convert.ToInt32(result["ss"]) * 1000 + Convert.ToInt32(result["fff"]);
                var info = result["lyric"];
                Lyrics.Add(new LyricDetailViewModel() { Position = totalMillisecond, Info = info });
            }
            Logger.Write(LogTypeEnum.消息, $"歌词解析完成");
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

            if (Lyrics.Count > 0)
            {
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
        }
        //暂停、恢复
        private void PlayerStateChange()
        {
            if (CurrentMusic == null)
            {
                DependencyService.Get<IToast>().Show("亲，当前没有歌曲可以播放");
                return;
            }

            if (IsPlaying == true)
            {
                Logger.Write(LogTypeEnum.消息, $"准备暂停");
                PlayerService.Instance().Pause();
            }
            else
            {
                Logger.Write(LogTypeEnum.消息, $"继续播放");
                PlayerService.Instance().Start();
            }

            IsPlaying = !IsPlaying;
        }
        //循环方式
        private async void RepeatTypeChange()
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
            await GlobalMethods.WritePlayerConfig();
        }
        /// <summary>
        /// 上一首
        /// </summary>
        private async void Previous()
        {
            if (CurrentMusic == null)
            {
                DependencyService.Get<IToast>().Show("目前这个歌曲似乎好像有问题哟~");
                return;
            }

            await PlayerService.Instance().Previous();
            await Logger.WriteAsync(LogTypeEnum.消息, $"上一首");
        }

        /// <summary>
        /// 下一首
        /// </summary>
        private async void Next()
        {
            if (CurrentMusic == null)
            {
                DependencyService.Get<IToast>().Show("目前这个歌曲似乎好像有问题哟~");
                return;
            }
            await PlayerService.Instance().Next();
            await Logger.WriteAsync(LogTypeEnum.消息, $"下一首");
        }
    }
}
