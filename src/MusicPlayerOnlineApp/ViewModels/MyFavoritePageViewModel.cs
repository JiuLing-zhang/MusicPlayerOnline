using System.Collections.ObjectModel;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Views;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;

        public Command AddMyFavoriteCommand => new Command(AddMyFavorite);
        public Command SelectedChangedCommand => new Command(SelectedChangedDo);
        public Command<MyFavoriteViewModel> EditFavoriteCommand => new Command<MyFavoriteViewModel>(EditFavorite);

        public string Title => "我的歌单";
        public MyFavoritePageViewModel()
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();

            _myFavoriteService = new MyFavoriteService();
        }

        public async void OnAppearing()
        {
            FavoriteList.Clear();
            var myFavoriteList = await _myFavoriteService.GetMyFavoriteList();
            foreach (var myFavorite in myFavoriteList)
            {
                FavoriteList.Add(new MyFavoriteViewModel()
                {
                    Id = myFavorite.Id,
                    Name = myFavorite.Name,
                    MusicCount = myFavorite.MusicCount,
                    ImageUrl = myFavorite.ImageUrl
                });
            }
        }

        private ObservableCollection<MyFavoriteViewModel> _favoriteList;
        public ObservableCollection<MyFavoriteViewModel> FavoriteList
        {
            get => _favoriteList;
            set
            {
                _favoriteList = value;
                OnPropertyChanged();
            }
        }

        private MyFavoriteViewModel _selectedResult;
        public MyFavoriteViewModel SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;
                OnPropertyChanged();
            }
        }

        private async void AddMyFavorite()
        {
            //TODO 跳转
            //_addMyFavoritePage.Initialize();
            //await Navigation.PushPopupAsync(_addMyFavoritePage);
        }

        private async void SelectedChangedDo()
        {
            if (SelectedResult.MusicCount == 0)
            {
                return;
            }
            //TODO 打开详情页
            // var myFavoriteDetailPage = new MyFavoriteDetailPage();
            // myFavoriteDetailPage.Initialize(
            //     new MyFavorite()
            //     {
            //         Id = myFavoriteView.Id,
            //         ImageUrl = myFavoriteView.ImageUrl,
            //         MusicCount = myFavoriteView.MusicCount,
            //         Name = myFavoriteView.Name
            //     });
            // await Navigation.PushAsync(myFavoriteDetailPage);
        }

        private async void EditFavorite(MyFavoriteViewModel myFavorite)
        {
            //TODO  初始化
            // _editMyFavoritePage.Initialize(myFavorite);
            // await Navigation.PushPopupAsync(_editMyFavoritePage);
        }
    }
}
