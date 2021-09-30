using System;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class AddMyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        public Command SaveMyFavoriteCommand => new Command(SaveMyFavorite);

        public AddMyFavoritePageViewModel()
        {
            _myFavoriteService = new MyFavoriteService();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private async void SaveMyFavorite()
        {
            if (Name.IsEmpty())
            {
                //TODO 提示
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }
            string id = Guid.NewGuid().ToString("d");
            var myFavorite = new MyFavorite()
            {
                Id = id,
                Name = Name,
                MusicCount = 0
            };

            await _myFavoriteService.Add(myFavorite);

            //TODO 返回
        }
    }
}
