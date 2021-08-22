using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Common;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Network.MusicProvider;
using MusicPlayerOnline.Player;

namespace MusicPlayerOnline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainWindowViewModel _myModel = new();
        //todo 工厂方式创建
        readonly IMusicProvider _myMusicProvider = new NeteaseMusicProvider();
        private readonly IPlayerProvider _player = new PlayerProvider();
        private readonly PlayerStateModel _playerState = new PlayerStateModel();

        public MainWindow()
        {
            InitializeComponent();

            LoadingAppConfig();
            BindingDataForUI();

            _playerState.IsPlaying = false;
            SetPlayerPlayMode();
            SetPlayOrPause();
            SetPlayerMute();
            SetPlayer();
        }
        private void LoadingAppConfig()
        {

        }

        /// <summary>
        /// UI数据绑定
        /// </summary>
        private void BindingDataForUI()
        {
            ListViewMusicSearchResult.ItemsSource = _myModel.MusicSearchResult;
            ListViewPlaylist.ItemsSource = _myModel.Playlist;
            DataContext = _myModel;
            _myModel.VoiceValue = 0.5;
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Messages.ShowInfo("设置");
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

            Task.Run(() =>
            {
                var result = _myMusicProvider.Search(_myModel.SearchKeyword).Result;
                if (result.Code != 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messages.ShowError(result.Message);
                    });
                    return;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _myModel.MusicSearchResult.Clear();
                    foreach (var musicInfo in result.Data)
                    {
                        _myModel.MusicSearchResult.Add(new SearchResultViewModel()
                        {
                            Id = musicInfo.Id,
                            Platform = musicInfo.Platform.GetDescription(),
                            Name = musicInfo.Name,
                            Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                            Artist = musicInfo.ArtistName,
                            PicUrl = musicInfo.PicUrl,
                            Album = musicInfo.AlbumName,
                            Duration = musicInfo.DurationText,
                            Fee = musicInfo.Fee
                        });
                    }
                });
            });
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var music = GetMusicInfoFromButtonTag(sender);
            if (music == null)
            {
                Messages.ShowError("播放失败：数据选取失败");
                return;
            }
            if (music.Fee == 1)
            {
                Messages.ShowWarning("暂时不支持VIP音乐的播放");
                return;
            }
            AddMusicToPlaylist(music);
            Play(music.Id);
        }

        private void BtnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var music = GetMusicInfoFromButtonTag(sender);
            if (music == null)
            {
                Messages.ShowError("添加失败：数据选取失败");
                return;
            }
            if (music.Fee == 1)
            {
                Messages.ShowWarning("暂时不支持VIP音乐的播放");
                return;
            }
            AddMusicToPlaylist(music);
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

            if (selectedMusic.Fee == 1)
            {
                Messages.ShowWarning("暂时不支持VIP音乐的播放");
                return;
            }
            AddMusicToPlaylist(selectedMusic);
            Play(selectedMusic.Id);
        }

        private void BtnDeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                Messages.ShowError("删除，读取信息异常");
                return;
            }

            int musicId = (int)btn.Tag;
            var music = _myModel.Playlist.FirstOrDefault(x => x.Id == musicId);
            _myModel.Playlist.Remove(music);
            _player.RemoveFromPlaylist(musicId);
        }

        private void Playlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            Play(selectedMusic.Id);
        }

        private void AddMusicToPlaylist(SearchResultViewModel music)
        {
            if (_myModel.Playlist.Any(x => x.Id == music.Id))
            {
                //已在播放列表包含，跳过
                return;
            }

            var result = _myMusicProvider.GetMusicUrl(music.Id).Result;
            if (result.Code != 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.ShowError(result.Message);
                });
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

            var data = new PlaylistModel()
            {
                Id = music.Id,
                Name = music.Name,
                Alias = music.Alias,
                ArtistName = music.Artist,
                AlbumName = music.Album,
                PlayUrl = result.Data,
                PicUrl = music.PicUrl
            };
            _player.AddToPlaylist(data);
        }
        private void Play(int musicId)
        {
            _player.PlayNew(musicId);
        }

        private void BtnChangePlayMode_Click(object sender, RoutedEventArgs e)
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
            SetPlayerPlayMode();
        }
        private void SoundOff_Click(object sender, RoutedEventArgs e)
        {
            _playerState.IsMute = !_playerState.IsMute;
            SetPlayerMute();
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
                _player.Play();
            }
            else
            {
                this.ImgPlayOrPause.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/play_64px.png"));
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
            _player.PlayMode = _playerState.PlayMode;
        }

        readonly DispatcherTimer _timer = new DispatcherTimer();
        private void SetPlayer()
        {
            _player.MusicStarted += _player_MusicStarted;
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
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

        private void _player_MusicStarted(PlaylistModel music)
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
            musicInfo = $"{musicInfo}{music.ArtistName}{Environment.NewLine}";
            //专辑
            musicInfo = $"{musicInfo}{music.AlbumName}";
            _myModel.CurrentMusicInfo = musicInfo;
            this.ImgCurrentMusic.Source = new BitmapImage(new Uri(music.PicUrl));
            _playerState.IsPlaying = true;
            SetPlayOrPause();
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            _player.Previous();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            _player.Next();
        }

        private void SliderVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _player.VoiceValue = _myModel.VoiceValue;
        }
    }
}