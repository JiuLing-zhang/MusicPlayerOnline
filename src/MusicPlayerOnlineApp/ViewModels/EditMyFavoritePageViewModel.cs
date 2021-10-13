using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    [QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
    public class EditMyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;

        public Command RenameCommand => new Command(Rename);
        public Command RemoveCommand => new Command(Remove);

        public EditMyFavoritePageViewModel()
        {
            _myFavoriteService = new MyFavoriteService();
        }

        private string _myFavoriteId;
        public string MyFavoriteId
        {
            get => _myFavoriteId;
            set
            {
                _myFavoriteId = value;
                OnPropertyChanged();

                GetMyFavoriteDetail();
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

        private string _newName;
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                OnPropertyChanged();
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        private async void GetMyFavoriteDetail()
        {
            var myFavorite = await _myFavoriteService.GetMyFavorite(MyFavoriteId);
            Name = myFavorite.Name;
            ImageUrl = myFavorite.ImageUrl.IsEmpty() ? "icon" : myFavorite.ImageUrl;
        }
        private async void Rename()
        {
            if (NewName.IsEmpty())
            {
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }

            var myFavorite = await _myFavoriteService.GetMyFavorite(MyFavoriteId);
            if (myFavorite.Name == NewName)
            {
                await Shell.Current.GoToAsync($"..", true);
                DependencyService.Get<IToast>().Show("修改成功");
                return;
            }

            myFavorite.Name = NewName;
            await _myFavoriteService.Update(myFavorite);
            await Shell.Current.GoToAsync($"..", true);
            DependencyService.Get<IToast>().Show("修改成功");
        }

        private async void Remove()
        {
            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }

            var result = await _myFavoriteService.DeleteMyFavorite(MyFavoriteId);
            await Shell.Current.GoToAsync($"..", true);
            DependencyService.Get<IToast>().Show("删除成功");
        }
    }
}
