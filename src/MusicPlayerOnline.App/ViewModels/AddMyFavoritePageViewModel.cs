using System;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{

    [QueryProperty(nameof(AddedMusicId), nameof(AddedMusicId))]
    public class AddMyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        private readonly IMusicService _musicService;

        public Command SaveMyFavoriteCommand => new Command(SaveMyFavorite);

        public AddMyFavoritePageViewModel()
        {
            _myFavoriteService = new MyFavoriteService();
            _musicService = new MusicService();
        }

        private string _addedMusicId;
        /// <summary>
        /// 要添加的歌曲ID
        /// </summary>
        public string AddedMusicId
        {
            get => _addedMusicId;
            set
            {
                _addedMusicId = value;
                OnPropertyChanged();
            }
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
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }

            var myFavorite = await _myFavoriteService.GetMyFavoriteByName(Name);
            if (myFavorite == null)
            {
                string id = Guid.NewGuid().ToString("d");
                myFavorite = new MyFavorite()
                {
                    Id = id,
                    Name = Name,
                    MusicCount = 0
                };
            }


            await _myFavoriteService.Add(myFavorite);

            if (AddedMusicId.IsEmpty())
            {
                //不需要添加歌曲到歌单时，直接退出
                await Shell.Current.GoToAsync($"..", true);
                return;
            }

            //添加完歌单后需要将歌曲添加到歌单
            var music = await _musicService.GetMusicDetail(AddedMusicId);
            var result = await _myFavoriteService.AddToMyFavorite(music, myFavorite.Id);
            if (result.Succeed == false)
            {
                DependencyService.Get<IToast>().Show("添加失败");
            }
            await Shell.Current.GoToAsync("../..", true);
        }
    }
}
