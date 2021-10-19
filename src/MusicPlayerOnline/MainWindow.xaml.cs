using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Common;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Network;
using MusicPlayerOnline.Player;
using MusicPlayerOnline.Service;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListViewItem = System.Windows.Controls.ListViewItem;
namespace MusicPlayerOnline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _myModel = new();
        readonly MusicNetPlatform _musicNetPlatform = new();
        private readonly IPlayerProvider _player = new PlayerProvider();
        private readonly PlayerStateModel _playerState = new();
        private readonly DispatcherTimer _timerPlayProgress = new();
        private NotifyIcon _notifyIcon;
        private readonly IPlaylistService _playlistService = new PlaylistService();
        private readonly IMusicService _musicService = new MusicService();

        private MusicDetail _currentMusic;
        public MainWindow()
        {
            InitializeComponent();

            LoadingAppConfig();
            BindingDataForUI();

            _playerState.IsPlaying = false;
            _myModel.SearchPlatform = 0;
            foreach (PlatformEnum item in Enum.GetValues(typeof(PlatformEnum)))
            {
                _myModel.SearchPlatform = _myModel.SearchPlatform | item;
            }

            InitializePlayProgressTimer();
            if (GlobalArgs.AppConfig.General.IsAutoCheckUpdate)
            {
                CheckUpdate();
            }

            InitializeNotifyIcon();

            GetPlaylist();
        }
        private void InitializeNotifyIcon()
        {
            //设置托盘的各个属性
            _notifyIcon = new NotifyIcon
            {
                Text = "在线音乐助手",
                Visible = true,
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath)
            };
            _notifyIcon.MouseClick += _notifyIcon_MouseClick;
        }

        private void _notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (this.Visibility == Visibility.Visible)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                this.Activate();
            }
        }

        private void InitializePlayProgressTimer()
        {
            _player.MediaEnded += _player_MediaEnded;
            _player.MusicMediaFailed += _player_MusicMediaFailed;
            _timerPlayProgress.Interval = TimeSpan.FromMilliseconds(1000);
            _timerPlayProgress.Tick += _timerPlayProgress_Tick;
        }

        private void LoadingAppConfig()
        {
            _playerState.IsMute = GlobalArgs.AppConfig.Player.IsSoundOff;
            _playerState.PlayMode = GlobalArgs.AppConfig.Player.PlayMode;
            _myModel.VoiceValue = GlobalArgs.AppConfig.Player.Voice;

            SetPlayerPlayMode();
            SetPlayOrPause();
            SetPlayerMute();
        }

        /// <summary>
        /// UI数据绑定
        /// </summary>
        private void BindingDataForUI()
        {
            ListViewMusicSearchResult.ItemsSource = _myModel.MusicSearchResult;
            ListViewPlaylist.ItemsSource = _myModel.Playlist;
            DataContext = _myModel;
        }

        private async void GetPlaylist()
        {
            var playlist = await _playlistService.GetList();
            foreach (var item in playlist)
            {
                _myModel.Playlist.Add(new PlaylistViewModel()
                {
                    Id = item.MusicDetailId,
                    MusicText = $"{item.Name}-{item.Artist}"
                });
            }
        }
        private void CheckUpdate()
        {
            Task.Run(() =>
            {
                try
                {
                    var (isNewVersion, version, link) = new CheckForUpdates().Check();
                    if (isNewVersion == false)
                    {
                        return;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messages.ShowInfo($"发现新版本{version}{System.Environment.NewLine}点击确定查看");
                    });
                    using Process compiler = new Process();
                    compiler.StartInfo.FileName = link;
                    compiler.StartInfo.UseShellExecute = true;
                    compiler.Start();
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messages.ShowInfo($"检查自动更新失败：{ex.Message}");
                    });
                }
            });
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalArgs.AppConfig.General.IsHideWindowWhenMinimize)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.ImgMaximize.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/maximize.png"));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.ImgMaximize.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/restore.png"));
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            var about = new SettingWindow
            {
                Owner = this
            };
            about.ShowDialog();
        }

        private void ReadyToSearch_Click(object sender, RoutedEventArgs e)
        {
            TxtKeyword.Focus();
        }

        private void TxtKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _myModel.SearchKeyword = "";
                return;
            }
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (_myModel.SearchKeyword.IsEmpty())
            {
                return;
            }

            _myModel.MusicSearchResult.Clear();
            Task.Run(() =>
            {
                _myModel.IsMusicSearching = true;
                try
                {
                    var musics = _musicNetPlatform.Search(_myModel.SearchPlatform, _myModel.SearchKeyword).Result;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (musics.Count == 0)
                        {
                            Application.Current.Dispatcher.Invoke(() => { Messages.ShowError("似乎啥也没找到，要不，您再试试？"); });
                            return;
                        }

                        foreach (var musicInfo in musics)
                        {
                            _myModel.MusicSearchResult.Add(new SearchResultViewModel()
                            {
                                Platform = musicInfo.Platform.GetDescription(),
                                Name = musicInfo.Name,
                                Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                                Artist = musicInfo.Artist,
                                Album = musicInfo.Album,
                                Duration = musicInfo.DurationText,
                                SourceData = musicInfo
                            });
                        }
                    });
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _myModel.IsMusicSearching = false;
                    });
                }
            });
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var selectedMusic = GetMusicInfoFromButtonTag(sender);
            if (selectedMusic == null)
            {
                Messages.ShowError("播放失败：数据选取失败");
                return;
            }

            BuildMusicDetail(selectedMusic.SourceData, musicDetail =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (musicDetail == null)
                    {
                        Messages.ShowError("操作失败，该歌曲的信息似乎没找到~~~");
                        return;
                    }
                    AddAndPlayMusic(musicDetail);
                });
            });
        }

        private void BtnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var selectedMusic = GetMusicInfoFromButtonTag(sender);
            if (selectedMusic == null)
            {
                Messages.ShowError("添加失败：数据选取失败");
                return;
            }

            BuildMusicDetail(selectedMusic.SourceData, musicDetail =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (musicDetail == null)
                    {
                        Messages.ShowError("操作失败，该歌曲的信息似乎没找到~~~");
                        return;
                    }
                    AddMusicToPlaylist(musicDetail);
                });
            });
        }

        private SearchResultViewModel GetMusicInfoFromButtonTag(object button)
        {
            var btn = button as Button;
            if (btn == null)
            {
                return null;
            }

            var searchMusic = btn.Tag as SearchResultViewModel;
            if (searchMusic == null)
            {
                return null;
            }
            return searchMusic;
        }

        private void SearchResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lvi = sender as ListViewItem;
            if (lvi == null)
            {
                Messages.ShowError("播放失败，读取信息异常");
                return;
            }

            var selectedMusic = lvi.DataContext as SearchResultViewModel;
            if (selectedMusic == null)
            {
                Messages.ShowError("播放失败，数据解析异常");
                return;
            }

            BuildMusicDetail(selectedMusic.SourceData, musicDetail =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (musicDetail == null)
                    {
                        Messages.ShowError("操作失败，该歌曲的信息似乎没找到~~~");
                        return;
                    }
                    AddAndPlayMusic(musicDetail);
                });
            });
        }

        private void BuildMusicDetail(MusicSearchResult music, Action<MusicDetail> callback)
        {
            Task.Run(() =>
            {
                var data = _musicNetPlatform.BuildMusicDetail(music).Result;
                callback(data);
            });
        }
        private void BtnDeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                Messages.ShowError("删除，读取信息异常");
                return;
            }

            string musicId = btn.Tag.ToString();
            var music = _myModel.Playlist.FirstOrDefault(x => x.Id == musicId);
            _myModel.Playlist.Remove(music);
            _playlistService.Delete(music.Id);
        }

        private async void Playlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lvi = sender as ListViewItem;
            if (lvi == null)
            {
                Messages.ShowError("播放失败，读取信息异常");
                return;
            }

            var selectedMusic = lvi.DataContext as PlaylistViewModel;
            if (selectedMusic == null)
            {
                Messages.ShowError("播放失败，数据解析异常");
                return;
            }

            var music = await _musicService.GetMusicDetail(selectedMusic.Id);
            PlayMusic(music);
        }

        /// <summary>
        /// 添加到播放列表并播放
        /// </summary>
        private void AddAndPlayMusic(MusicDetail music)
        {
            AddMusicToPlaylist(music);
            PlayMusic(music);
        }

        /// <summary>
        /// 添加到播放列表
        /// </summary>
        /// <param name="music"></param>
        private void AddMusicToPlaylist(MusicDetail music)
        {
            if (_myModel.Playlist.Any(x => x.Id == music.Id))
            {
                //已在播放列表包含，跳过
                return;
            }

            string toolTip = "";
            if (music.Alias.IsNotEmpty())
            {
                toolTip = $"别名：（{music.Alias}）{Environment.NewLine}";
            }
            toolTip = $"{toolTip}歌手：{music.Artist}{Environment.NewLine}专辑：{music.Album}";
            _myModel.Playlist.Add(new PlaylistViewModel()
            {
                Id = music.Id,
                MusicText = $"{music.Name} - {music.Artist}",
                MusicToolTip = toolTip
            });
            _playlistService.Add(music);
            _musicService.Add(music);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void PlayMusic(MusicDetail music)
        {
            Task.Run(() =>
            {
                if (music.Platform == PlatformEnum.Netease)
                {
                    music = _musicNetPlatform.UpdateMusicDetail(music).Result;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in _myModel.Playlist)
                    {
                        if (item.IsPlaying == true)
                        {
                            item.IsPlaying = false;
                            break;
                        }
                    }
                    var playlistMusic = _myModel.Playlist.FirstOrDefault(x => x.Id == music.Id);
                    if (playlistMusic != null)
                    {
                        playlistMusic.IsPlaying = true;
                    }

                    //名称
                    string musicInfo = $"{music.Name}{Environment.NewLine}";
                    //别名
                    if (music.Alias.IsNotEmpty())
                    {
                        musicInfo = $"{musicInfo}{music.Alias}{Environment.NewLine}";
                    }
                    //歌手
                    musicInfo = $"{musicInfo}{music.Artist}{Environment.NewLine}";
                    //专辑
                    musicInfo = $"{musicInfo}{music.Album}";
                    _myModel.CurrentMusicInfo = musicInfo;
                    this.ImgCurrentMusic.Source = new BitmapImage(new Uri(music.ImageUrl));
                    _playerState.IsPlaying = true;
                    SetPlayOrPause();

                    _currentMusic = music;
                    _player.Play(music);
                    _timerPlayProgress.Start();
                });
            });
        }

        private async void BtnChangePlayMode_Click(object sender, RoutedEventArgs e)
        {
            if (_playerState.PlayMode == PlayModeEnum.RepeatOne)
            {
                _playerState.PlayMode = PlayModeEnum.RepeatList;
            }
            else if (_playerState.PlayMode == PlayModeEnum.RepeatList)
            {
                _playerState.PlayMode = PlayModeEnum.Shuffle;
            }
            else if (_playerState.PlayMode == PlayModeEnum.Shuffle)
            {
                _playerState.PlayMode = PlayModeEnum.RepeatOne;
            }
            GlobalArgs.AppConfig.Player.PlayMode = _playerState.PlayMode;
            SetPlayerPlayMode();
            await GlobalMethods.WritePlayerConfig();
        }
        private async void SoundOff_Click(object sender, RoutedEventArgs e)
        {
            _playerState.IsMute = !_playerState.IsMute;
            GlobalArgs.AppConfig.Player.IsSoundOff = _playerState.IsMute;
            SetPlayerMute();
            await GlobalMethods.WritePlayerConfig();
        }
        private void SetPlayerMute()
        {
            if (_playerState.IsMute == true)
            {
                this.ImgSoundOff.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/mute.png"));
            }
            else
            {
                this.ImgSoundOff.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/voice.png"));
            }

            _player.IsMuted = _playerState.IsMute;
        }
        private void PlayOrPause_Click(object sender, RoutedEventArgs e)
        {
            _playerState.IsPlaying = !_playerState.IsPlaying;
            SetPlayOrPause();
        }
        private void SetPlayOrPause()
        {
            if (_playerState.IsPlaying == true)
            {
                this.ImgPlayOrPause.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/pause64px.png"));
                _timerPlayProgress.Start();
                _player.Play();
            }
            else
            {
                this.ImgPlayOrPause.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/play_64px.png"));
                _timerPlayProgress.Stop();
                _player.Pause();
            }
        }
        private void SetPlayerPlayMode()
        {
            if (_playerState.PlayMode == PlayModeEnum.RepeatOne)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/repeat_one.png"));
                this.ImgPlayMode.ToolTip = "当前状态：单曲循环";
            }
            else if (_playerState.PlayMode == PlayModeEnum.RepeatList)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/repeat.png"));
                this.ImgPlayMode.ToolTip = "当前状态：列表循环";
            }
            else if (_playerState.PlayMode == PlayModeEnum.Shuffle)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/shuffle.png"));
                this.ImgPlayMode.ToolTip = "当前状态：随机播放";
            }
        }
        private void _timerPlayProgress_Tick(object sender, EventArgs e)
        {
            var result = _player.GetPosition();
            if (result.isPlaying == false)
            {
                return;
            }

            _myModel.PlayedTime = $"{result.position.Minutes}:{result.position.Seconds:D2}";
            _myModel.TotalTime = $"{result.total.Minutes}:{result.total.Seconds:D2}";
            _myModel.PlayPercent = result.percent;
        }

        private void _player_MediaEnded()
        {
            Next();
        }

        private void _player_MusicMediaFailed()
        {
            if (GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed == false)
            {
                return;
            }
            Next();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private async void Previous()
        {
            if (_playerState.PlayMode == PlayModeEnum.RepeatOne)
            {
                PlayMusic(_currentMusic);
                return;
            }
            if (_playerState.PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < _myModel.Playlist.Count; i++)
                {
                    if (_myModel.Playlist[i].Id == _currentMusic.Id)
                    {
                        nextId = i - 1;
                        break;
                    }
                }
                //列表第一首
                if (nextId < 0)
                {
                    nextId = _myModel.Playlist.Count - 1;
                }
                var music = await _musicService.GetMusicDetail(_myModel.Playlist[nextId].Id);
                PlayMusic(music);
                return;
            }
            if (_playerState.PlayMode == PlayModeEnum.Shuffle)
            {
                if (_myModel.Playlist.Count <= 1)
                {
                    PlayMusic(_currentMusic);
                    return;
                }

                PlaylistViewModel randomMusic;
                do
                {
                    randomMusic = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<PlaylistViewModel>(_myModel.Playlist);
                } while (randomMusic.Id == _currentMusic.Id);
                var music = await _musicService.GetMusicDetail(randomMusic.Id);
                PlayMusic(music);
            }
        }

        private async void Next()
        {
            if (_playerState.PlayMode == PlayModeEnum.RepeatOne)
            {
                PlayMusic(_currentMusic);
                return;
            }
            if (_playerState.PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < _myModel.Playlist.Count; i++)
                {
                    if (_myModel.Playlist[i].Id == _currentMusic.Id)
                    {
                        nextId = i + 1;
                        break;
                    }
                }
                //列表最后一首
                if (_myModel.Playlist.Count == nextId)
                {
                    nextId = 0;
                }
                var music = await _musicService.GetMusicDetail(_myModel.Playlist[nextId].Id);
                PlayMusic(music);
                return;
            }
            if (_playerState.PlayMode == PlayModeEnum.Shuffle)
            {
                if (_myModel.Playlist.Count <= 1)
                {
                    PlayMusic(_currentMusic);
                    return;
                }

                PlaylistViewModel randomMusic = null;
                do
                {
                    randomMusic = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<PlaylistViewModel>(_myModel.Playlist);
                } while (randomMusic.Id == _currentMusic.Id);
                var music = await _musicService.GetMusicDetail(randomMusic.Id);
                PlayMusic(music);
            }
        }


        private async void SliderVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _player.VoiceValue = _myModel.VoiceValue;
            GlobalArgs.AppConfig.Player.Voice = _myModel.VoiceValue;
            await GlobalMethods.WritePlayerConfig();
        }

        private void SliderPlayProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _player.SetProgress(_myModel.PlayPercent);
            _timerPlayProgress.Start();
        }

        private void SliderPlayProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _timerPlayProgress.Stop();
        }

    }
}