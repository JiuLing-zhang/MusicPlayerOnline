using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Views;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.ViewModels
{
    [QueryProperty(nameof(AddedMusicId), nameof(AddedMusicId))]
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
            await Shell.Current.GoToAsync($"{nameof(AddMyFavoritePage)}?{nameof(AddMyFavoritePageViewModel.AddedMusicId)}={AddedMusicId}", true);
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
                await Shell.Current.GoToAsync($"..", true);
                DependencyService.Get<IToast>().Show("添加失败");
                return;
            }
            await Shell.Current.GoToAsync($"..", true);
            DependencyService.Get<IToast>().Show("添加成功");
        }
    }
}
