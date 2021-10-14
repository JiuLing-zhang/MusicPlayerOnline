using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Views;
using System.Collections.ObjectModel;
using MusicPlayerOnlineApp.AppInterface;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class PlaylistPageViewModel : ViewModelBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;

        public Command<MusicDetailViewModel> AddToMyFavoriteCommand => new Command<MusicDetailViewModel>(AddToMyFavorite);
        public Command ClearPlaylistCommand => new Command(ClearPlaylist);
        public PlaylistPageViewModel()
        {
            Playlist = new ObservableCollection<MusicDetailViewModel>();

            _playlistService = new PlaylistService();
            _musicService = new MusicService();

            MessagingCenter.Subscribe<SearchResultPageViewModel>(this, SubscribeKey.UpdatePlaylist, (_) =>
            {
                GetPlaylist();
            });
            MessagingCenter.Subscribe<MyFavoriteDetailPageViewModel>(this, SubscribeKey.UpdatePlaylist, (_) =>
            {
                GetPlaylist();
            });
            GetPlaylist();
        }
        public void OnAppearing()
        {
            if (SearchKeyword.IsNotEmpty())
            {
                SearchKeyword = "";
            }
        }
        private async void GetPlaylist()
        {
            GlobalMethods.ShowLoading();
            if (Playlist.Count > 0)
            {
                Playlist.Clear();
            }
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

            GlobalMethods.HideLoading();
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

        private MusicDetailViewModel _selectedItem;
        public MusicDetailViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

                SelectedChangedDo();
            }
        }

        public async void Search()
        {
            if (SearchKeyword.IsEmpty())
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(SearchResultPage)}?{nameof(SearchResultPageViewModel.SearchKeyword)}={SearchKeyword}", true);
        }

        private async void SelectedChangedDo()
        {
            if (SelectedItem == null)
            {
                return;
            }
            var music = await _musicService.GetMusicDetail(SelectedItem.Id);
            if (music == null)
            {
                DependencyService.Get<IToast>().Show("获取歌曲信息失败");
                return;
            }
            GlobalMethods.PlayMusic(music);
            await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);
        }

        private async void AddToMyFavorite(MusicDetailViewModel music)
        {
            if (music == null)
            {
                return;
            }
            
            await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
        }

        public async void RemovePlaylistItem(MusicDetailViewModel music)
        {
            if (music == null)
            {
                return;
            }

            await _playlistService.Delete(music.Id);
            GetPlaylist();
        }

        private async void ClearPlaylist()
        {
            if (Playlist.Count == 0)
            {
                DependencyService.Get<IToast>().Show("别删除了，播放列表是空哒");
                return;
            }

            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除播放列表吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }
            await _playlistService.Clear();
            DependencyService.Get<IToast>().Show("播放列表已删除");
            GetPlaylist();
        }
    }
}
