using System;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMyFavoritePage : ContentPage
    {
        private readonly AddMyFavoritePageViewModel _myModel = new AddMyFavoritePageViewModel();
        public Action<string> SaveFinished;
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
            await Shell.Current.GoToAsync($"../..");
            return;
            string name = _myModel.Name;
            if (name.IsEmpty())
            {
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }

            if (DatabaseProvide.Database.Table<MyFavorite>().Where(x => x.Name == name).CountAsync().Result > 0)
            {
                DependencyService.Get<IToast>().Show("该歌单已存在");
                await Navigation.PopPopupAsync();
                return;
            }

            string id = Guid.NewGuid().ToString("d");
            var myFavorite = new MyFavorite()
            {
                Id = id,
                Name = name,
                MusicCount = 0
            };

            await DatabaseProvide.Database.InsertAsync(myFavorite);
            await Navigation.PopPopupAsync();
            SaveFinished.Invoke(id);
        }
    }
}