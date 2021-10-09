﻿using System.Collections.ObjectModel;
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
            if (FavoriteList.Count > 0)
            {
                FavoriteList.Clear();
            }
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
            await Shell.Current.GoToAsync(nameof(AddMyFavoritePage));
        }

        private async void SelectedChangedDo()
        {
            if (SelectedResult.MusicCount == 0)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={SelectedResult.Id}");
        }

        private async void EditFavorite(MyFavoriteViewModel myFavorite)
        {
            await Shell.Current.GoToAsync($"{nameof(EditMyFavoritePage)}?{nameof(EditMyFavoritePageViewModel.MyFavoriteId)}={myFavorite.Id}");
        }
    }
}
