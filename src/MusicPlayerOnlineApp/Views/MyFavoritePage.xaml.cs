using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            this.Appearing += async (sender, e) =>
            {
                await LoadMyFavoriteList();
            };
            _addMyFavoritePage.SaveFinished = async (myFavoriteId) =>
            {
                await LoadMyFavoriteList();
            };
            //TODO 绑定事件
            // _editMyFavoritePage.EditFinished = async () =>
            // {
            //     await LoadMyFavoriteList();
            // };
        }

        private async Task LoadMyFavoriteList()
        {
            _myModel.FavoriteList.Clear();
            var myFavoriteList = await DatabaseProvide.Database.Table<MyFavorite>().ToListAsync();
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
        }

        private async void OnAddFavorite_Clicked(object sender, EventArgs e)
        {
            //TODO 跳转
            //_addMyFavoritePage.Initialize();
            //await Navigation.PushPopupAsync(_addMyFavoritePage);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyFavoriteViewModel myFavoriteView = e.CurrentSelection[0] as MyFavoriteViewModel;
            if (myFavoriteView.MusicCount == 0)
            {
                return;
            }
            var myFavoriteDetailPage = new MyFavoriteDetailPage();
            myFavoriteDetailPage.Initialize(
                new MyFavorite()
                {
                    Id = myFavoriteView.Id,
                    ImageUrl = myFavoriteView.ImageUrl,
                    MusicCount = myFavoriteView.MusicCount,
                    Name = myFavoriteView.Name
                });
            await Navigation.PushAsync(myFavoriteDetailPage);
        }

        private async void BtnEditFavorite_Clicked(object sender, EventArgs e)
        {
            var myFavoriteId = (sender as ImageButton).ClassId;
            var myFavorite = await DatabaseProvide.Database.GetAsync<MyFavorite>(myFavoriteId);
            //TODO  初始化
            // _editMyFavoritePage.Initialize(myFavorite);
            // await Navigation.PushPopupAsync(_editMyFavoritePage);
        }
    }
}