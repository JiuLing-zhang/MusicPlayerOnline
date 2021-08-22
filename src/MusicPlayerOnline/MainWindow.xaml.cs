using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Common;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Network;
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
        private PlayModeEnum _playMode = PlayModeEnum.RepeatList;

        public MainWindow()
        {
            InitializeComponent();

            LoadingAppConfig();
            BindingDataForUI();

            SetPlayMode();
            _player.PlaylistChanged += Player_PlaylistChanged;
        }

        private void Player_PlaylistChanged(object sender)
        {
            var playlist = sender as List<PlaylistModel>;
            if (playlist == null)
            {
                Messages.ShowWarning("更新播放列表失败");
                return;
            }

            _myModel.Playlist.Clear();
            foreach (var item in playlist)
            {
                string toolTip = "";
                if (item.Alias.IsNotEmpty())
                {
                    toolTip = $"别名：（{item.Alias}）{Environment.NewLine}";
                }
                toolTip = $"{toolTip}歌手：{item.ArtistName}{Environment.NewLine}专辑：{item.AlbumName}";
                _myModel.Playlist.Add(new PlaylistViewModel()
                {
                    Id = item.Id,
                    IsPlaying = item.IsPlaying,
                    MusicText = $"{item.Name} - {item.ArtistName}",
                    MusicToolTip = toolTip
                });
            }
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
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // this.DragMove();
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
            _player.RemoveFromPlaylist((int)btn.Tag);
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
            var result = _myMusicProvider.GetMusicUrl(music.Id).Result;
            if (result.Code != 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.ShowError(result.Message);
                });
                return;
            }
      
            var data = new PlaylistModel()
            {
                Id = music.Id,
                Name = music.Name,
                Alias = music.Alias,
                ArtistName = music.Artist,
                AlbumName = music.Album,
                PlayUrl = result.Data
            };
            _player.AddToPlaylist(data);
        }
        private void Play(int musicId)
        {
            _player.Play(musicId);
        }

        private void BtnChangePlayMode_Click(object sender, RoutedEventArgs e)
        {
            if (_playMode == PlayModeEnum.RepeatOne)
            {
                _playMode = PlayModeEnum.RepeatList;
            }
            else if (_playMode == PlayModeEnum.RepeatList)
            {
                _playMode = PlayModeEnum.Shuffle;
            }
            else if (_playMode == PlayModeEnum.Shuffle)
            {
                _playMode = PlayModeEnum.RepeatOne;
            }
            SetPlayMode();
        }

        private void SetPlayMode()
        {
            if (_playMode == PlayModeEnum.RepeatOne)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/repeat_one.png"));
                this.ImgPlayMode.ToolTip = "当前状态：单曲循环";
            }
            else if (_playMode == PlayModeEnum.RepeatList)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/repeat.png"));
                this.ImgPlayMode.ToolTip = "当前状态：列表循环";
            }
            else if (_playMode == PlayModeEnum.Shuffle)
            {
                this.ImgPlayMode.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/shuffle.png"));
                this.ImgPlayMode.ToolTip = "当前状态：随机播放";
            }
            _player.SetPlayMode(_playMode);
        }
    }
}
