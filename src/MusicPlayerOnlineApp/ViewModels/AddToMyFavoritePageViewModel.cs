using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.Views;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    [QueryProperty(nameof(AddedMusicId), nameof(AddedMusicId))]
    [QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
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
                GetMusicDetail();
            }
        }

        private string _myFavoriteId;
        /// <summary>
        /// 新添加的歌单名称
        /// </summary>
        public string MyFavoriteId
        {
            get => _myFavoriteId;
            set
            {
                _myFavoriteId = value;
                OnPropertyChanged();
                AddToNewMyFavorite();
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
            AddedMusic = await _musicService.GetMusicDetail(AddedMusicId);
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
                DependencyService.Get<IToast>().Show("添加失败");
            }
        }

        /// <summary>
        /// 将歌曲添加到新增的歌单中
        /// </summary>
        private async void AddToNewMyFavorite()
        {
            if (MyFavoriteId.IsEmpty())
            {
                return;
            }
            var result = await _myFavoriteService.AddToMyFavorite(AddedMusic, MyFavoriteId);
            if (result.Succeed == false)
            {
                DependencyService.Get<IToast>().Show("添加失败");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}
