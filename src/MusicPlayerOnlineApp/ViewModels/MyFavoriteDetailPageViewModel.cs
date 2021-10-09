using MusicPlayerOnline.Service;
using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class MyFavoriteDetailPageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        public MyFavoriteDetailPageViewModel()
        {
            MyFavoriteMusics = new ObservableCollection<MusicDetailViewModel>();

            _myFavoriteService = new MyFavoriteService();
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
            Title = $"歌单：{myFavorite.Name}";
        }
    }
}
