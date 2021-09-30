using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Views;
using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        }

        public async void OnAppearing()
        {
            SearchKeyword = "";
            Playlist.Clear();
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
                //TODO 提示
                return;
            }

            if (File.Exists(music.CachePath))
            {
                GlobalArgs.CurrentMusic = music;
                Common.GlobalArgs.Audio.Play(music.CachePath);
                return;
            }

            //TODO 加入判断 非WIFI是否允许播放
            var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
            var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
            if (!connectionTypes.Contains(wifi))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().Show("没有WIFI，不能播放");
                });
                return;
            }

            string cachePath = Path.Combine(Common.GlobalArgs.AppMusicCachePath, music.Id);
            await _musicService.CacheMusic(music, cachePath);

            Common.GlobalArgs.Audio.Play(cachePath);
        }

        private async void AddToMyFavorite(MusicDetailViewModel music)
        {
            if (music == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedId)}={music.Id}");
        }
    }
}
