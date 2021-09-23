using System;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnlineApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMyFavoritePage : PopupPage
    {
        private readonly AddMyFavoritePageViewModel _myModel = new AddMyFavoritePageViewModel();
        public Action SaveFinished;
        public AddMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }

        public void Initialize()
        {
            _myModel.Name = "";
        }
        private async void BtnSaveMyFavorite_Clicked(object sender, System.EventArgs e)
        {
            string name = _myModel.Name;
            if (name.IsEmpty())
            {
                await DisplayAlert("提示", "输入歌单名称", "确定");
                return;
            }

            if (DatabaseProvide.Database.Table<MyFavorite>().Where(x => x.Name == name).CountAsync().Result > 0)
            {
                await DisplayAlert("提示", "该歌单已存在", "确定");
                await Navigation.PopPopupAsync();
                return;
            }

            var myFavorite = new MyFavorite()
            {
                Id = Guid.NewGuid().ToString("d"),
                Name = name,
                MusicCount = 0
            };
            
            await DatabaseProvide.Database.InsertAsync(myFavorite);
            await Navigation.PopPopupAsync();
            SaveFinished.Invoke();
        }
    }
}