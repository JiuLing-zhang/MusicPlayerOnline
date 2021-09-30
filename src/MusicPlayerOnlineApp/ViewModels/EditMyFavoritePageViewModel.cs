using System;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
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

        private async void Rename()
        {
            if (NewName.IsEmpty())
            {
                //TODO 提示
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }

            var myFavorite = await _myFavoriteService.GetMyFavorite(MyFavoriteId);
            if (myFavorite.Name == NewName)
            {
                //TODO 直接提示成功
                return;
            }

            myFavorite.Name = NewName;
            await _myFavoriteService.Update(myFavorite);
            //TODO 提示，返回
        }

        private async void Remove()
        {
            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }

            //TODO 给ID赋值
            var result = await _myFavoriteService.DeleteMyFavorite("");
            //TODO 判断状态，返回页面

            DependencyService.Get<IToast>().Show("删除成功");
        }
    }
}
