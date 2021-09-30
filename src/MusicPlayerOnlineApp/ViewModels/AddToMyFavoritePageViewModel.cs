using System;
using System.Collections.ObjectModel;
using System.IO;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Views;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    [QueryProperty(nameof(AddedId), nameof(AddedId))]
    public class AddToMyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        private readonly IMusicService _musicService;

        public Command AddMyFavoriteCommand => new Command(AddMyFavorite);
        public Command SelectedChangedCommand => new Command(SelectedChangedDo);
        public AddToMyFavoritePageViewModel()
        {
            MyFavoriteList = new ObservableCollection<MyFavoriteViewModel>();

            _myFavoriteService = new MyFavoriteService();
            _musicService = new MusicService();

            BindingMyFavoriteList();
        }

        private string _addedId;
        public string AddedId
        {
            get => _addedId;
            set
            {
                _addedId = value;
                OnPropertyChanged();
                GetMusicDetail();
            }
        }

        private MusicDetail _addedMusic;
        public MusicDetail AddedMusic
        {
            get => _addedMusic;
            set
            {
                _addedMusic = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MyFavoriteViewModel> _myFavoriteList;
        public ObservableCollection<MyFavoriteViewModel> MyFavoriteList
        {
            get => _myFavoriteList;
            set
            {
                _myFavoriteList = value;
                OnPropertyChanged();
            }
        }

        private MyFavoriteViewModel _selectedItem;
        public MyFavoriteViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        private async void GetMusicDetail()
        {
            AddedMusic = await _musicService.GetMusicDetail(AddedId);
        }

        private async void BindingMyFavoriteList()
        {
            MyFavoriteList.Clear();
            var myFavoriteList = await _myFavoriteService.GetMyFavoriteList();
            foreach (var myFavorite in myFavoriteList)
            {
                MyFavoriteList.Add(new MyFavoriteViewModel()
                {
                    Id = myFavorite.Id,
                    Name = myFavorite.Name,
                    MusicCount = myFavorite.MusicCount,
                    ImageUrl = myFavorite.ImageUrl
                });
            }
        }

        private async void AddMyFavorite()
        {
            await Shell.Current.GoToAsync($"{nameof(AddMyFavoritePage)}");
        }
        private async void SelectedChangedDo()
        {
            if (AddedMusic == null)
            {
                return;
            }
            var result = await _myFavoriteService.AddToMyFavorite(AddedMusic, SelectedItem.Id);
            if (result.Succeed == false)
            {
                //TODO 错误提醒
            }
        }
    }
}
