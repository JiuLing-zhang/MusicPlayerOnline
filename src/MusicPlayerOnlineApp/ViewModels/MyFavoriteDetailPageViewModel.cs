using MusicPlayerOnline.Service;
using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Views;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    [QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
    public class MyFavoriteDetailPageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;
        public Command SelectedChangedCommand => new Command(SelectedChangedDo);
        public Command PlayAllMusicsCommand => new Command(PlayAllMusics);

        public MyFavoriteDetailPageViewModel()
        {
            MyFavoriteMusics = new ObservableCollection<MusicDetailViewModel>();

            _myFavoriteService = new MyFavoriteService();
            _playlistService = new PlaylistService();
            _musicService = new MusicService();
        }

        private string _title;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _myFavoriteId;
        public string MyFavoriteId
        {
            get => _myFavoriteId;
            set
            {
                _myFavoriteId = value;
                OnPropertyChanged();

                LoadPageTitle();
                GetMyFavoriteDetail();
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
            }
        }

        private ObservableCollection<MusicDetailViewModel> _myFavoriteMusics;
        public ObservableCollection<MusicDetailViewModel> MyFavoriteMusics
        {
            get => _myFavoriteMusics;
            set
            {
                _myFavoriteMusics = value;
                OnPropertyChanged();
            }
        }

        private async void GetMyFavoriteDetail()
        {
            MyFavoriteMusics.Clear();
            var myFavoriteDetailList = await _myFavoriteService.GetMyFavoriteDetail(MyFavoriteId);
            int seq = 0;
            foreach (var myFavoriteDetail in myFavoriteDetailList)
            {
                MyFavoriteMusics.Add(new MusicDetailViewModel()
                {
                    Seq = ++seq,
                    Id = myFavoriteDetail.MusicId,
                    Platform = myFavoriteDetail.Platform.GetDescription(),
                    Artist = myFavoriteDetail.Artist,
                    Album = myFavoriteDetail.Album,
                    Name = myFavoriteDetail.Name
                });
            }
        }

        private async void LoadPageTitle()
        {
            var myFavorite = await _myFavoriteService.GetMyFavorite(MyFavoriteId);
            Title = myFavorite.Name;
        }

        private async void SelectedChangedDo()
        {
            var music = await _musicService.GetMusicDetail(SelectedItem.Id);
            if (music == null)
            {
                DependencyService.Get<IToast>().Show("获取歌曲信息失败");
                return;
            }

            await _playlistService.Add(music);

            GlobalMethods.PlayMusic(music);

            await Shell.Current.GoToAsync($"../{nameof(PlayingPage)}", true);
            MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
        }

        private async void PlayAllMusics()
        {
            GlobalMethods.ShowLoading();

            await _playlistService.Clear();

            int index = 0;
            foreach (var myFavoriteMusic in MyFavoriteMusics)
            {
                var music = await _musicService.GetMusicDetail(myFavoriteMusic.Id);
                if (music == null)
                {
                    DependencyService.Get<IToast>().Show("获取歌曲信息失败");
                    return;
                }
                await _playlistService.Add(music);
                if (index == 0)
                {
                    GlobalMethods.PlayMusic(music);
                }
                index++;
            }
            GlobalMethods.HideLoading();

            await Shell.Current.GoToAsync($"../{nameof(PlayingPage)}", true);
            MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
        }
    }
}
