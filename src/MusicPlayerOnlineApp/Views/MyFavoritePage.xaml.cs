using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFavoritePage : ContentPage
    {
        private readonly MyFavoritePageViewModel _myModel = new MyFavoritePageViewModel();
        private readonly AddMyFavoritePage _addMyFavoritePage = new AddMyFavoritePage();
        private readonly EditMyFavoritePage _editMyFavoritePage = new EditMyFavoritePage();

        public MyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            this.Appearing += (sender, e) =>
            {
                LoadMyFavoriteList();
            };
            _addMyFavoritePage.SaveFinished = LoadMyFavoriteList;
            _editMyFavoritePage.EditFinished = LoadMyFavoriteList;
        }

        private async void LoadMyFavoriteList()
        {
            await Task.Run(() =>
            {
                _myModel.FavoriteList.Clear();
                var myFavoriteList = DatabaseProvide.Database.Table<MyFavorite>().ToListAsync().Result;
                foreach (var myFavorite in myFavoriteList)
                {
                    _myModel.FavoriteList.Add(new MyFavoriteViewModel()
                    {
                        Id = myFavorite.Id,
                        Name = myFavorite.Name,
                        MusicCount = myFavorite.MusicCount,
                        ImageUrl = myFavorite.ImageUrl
                    });
                }
            });
        }

        private async void OnAddFavorite_Clicked(object sender, EventArgs e)
        {
            _addMyFavoritePage.Initialize();
            await Navigation.PushPopupAsync(_addMyFavoritePage);
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void BtnEditFavorite_Clicked(object sender, EventArgs e)
        {
            var myFavoriteId = (sender as ImageButton).ClassId;
            var myFavorite = await DatabaseProvide.Database.GetAsync<MyFavorite>(myFavoriteId);
            _editMyFavoritePage.Initialize(myFavorite);
            await Navigation.PushPopupAsync(_editMyFavoritePage);
        }
    }
}