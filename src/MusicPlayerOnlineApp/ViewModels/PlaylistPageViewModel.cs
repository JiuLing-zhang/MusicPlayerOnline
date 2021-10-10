using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Views;
using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.AppInterface;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class PlaylistPageViewModel : ViewModelBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;

        public Command<MusicDetailViewModel> AddToMyFavoriteCommand => new Command<MusicDetailViewModel>(AddToMyFavorite);
        public Command SelectedChangedCommand => new Command(SelectedChangedDo);
        public PlaylistPageViewModel()
        {
            Playlist = new ObservableCollection<MusicDetailViewModel>();

            _playlistService = new PlaylistService();
            _musicService = new MusicService();

            MessagingCenter.Subscribe<SearchResultPageViewModel, MusicDetail>(this, SubscribeKey.SearchFinished, (_, data) =>
            {
                SearchKeyword = "";
                if (data == null)
                {
                    return;
                }
                Playlist.Add(new MusicDetailViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Artist = data.Artist
                });
            });

            GetPlaylist();
        }

        private async void GetPlaylist()
        {
            var playlist = await _playlistService.GetList();
            foreach (var item in playlist)
            {
                Playlist.Add(new MusicDetailViewModel()
                {
                    Id = item.MusicDetailId,
                    Name = item.Name,
                    Artist = item.Artist
                });
            }
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "播放列表";

        private string _searchKeyword;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MusicDetailViewModel> _playlist;
        /// <summary>
        /// 搜索到的结果列表
        /// </summary>
        public ObservableCollection<MusicDetailViewModel> Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        private MusicDetailViewModel _selectedResult;
        public MusicDetailViewModel SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;
                OnPropertyChanged();
            }
        }

        public async void Search()
        {
            if (SearchKeyword.IsEmpty())
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(SearchResultPage)}?{nameof(SearchResultPageViewModel.SearchKeyword)}={SearchKeyword}");
        }

        private async void SelectedChangedDo()
        {
            var music = await _musicService.GetMusicDetail(SelectedResult.Id);
            if (music == null)
            {
                DependencyService.Get<IToast>().Show("获取歌曲信息失败");
                return;
            }

            if (File.Exists(music.CachePath))
            {
                GlobalMethods.PlayMusic(music);
                return;
            }

            var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
            var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
            if (!connectionTypes.Contains(wifi) && GlobalArgs.AppConfig.General.IsWifiPlayOnly)
            {
                DependencyService.Get<IToast>().Show("仅在WIFI下允许播放");
                return;
            }

            string cachePath = Path.Combine(Common.GlobalArgs.AppMusicCachePath, music.Id);
            await _musicService.CacheMusic(music, cachePath);
            music.CachePath = cachePath;
            Common.GlobalMethods.PlayMusic(music);
        }

        private async void AddToMyFavorite(MusicDetailViewModel music)
        {
            if (music == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}");
        }
    }
}
