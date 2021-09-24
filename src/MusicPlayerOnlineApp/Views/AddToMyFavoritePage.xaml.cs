using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToMyFavoritePage : PopupPage
    {
        private readonly AddToMyFavoritePageViewModel _myModel = new AddToMyFavoritePageViewModel();
        private readonly AddMyFavoritePage _addMyFavoritePage = new AddMyFavoritePage();
        public Action AddFinished;
        private MusicDetail _music;
        public AddToMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            _addMyFavoritePage.SaveFinished = AddMusicToMyFavorite;
        }
        public void Initialize(MusicDetail music)
        {
            BindingMyFavoriteList();
            _music = music;
        }

        private async void AddMusicToMyFavorite(string myFavoriteId)
        {
            if (DatabaseProvide.Database.Table<MyFavoriteDetail>().Where(x => x.MyFavoriteId == myFavoriteId && x.MusicId == _music.Id).CountAsync().Result > 0)
            {
                DependencyService.Get<IToast>().Show("不能重复添加");
                await Navigation.PopPopupAsync();
                return;
            }
            string id = Guid.NewGuid().ToString("d");
            var obj = new MyFavoriteDetail()
            {
                Id = id,
                MyFavoriteId = myFavoriteId,
                MusicId = _music.Id,
                Platform = _music.Platform,
                Name = _music.Name,
                Artist = _music.Artist,
                Album = _music.Album
            };
            int count = await DatabaseProvide.Database.InsertAsync(obj);
            if (count == 0)
            {
                DependencyService.Get<IToast>().Show("添加失败");
                await Navigation.PopPopupAsync();
                return;
            }

            var myFavorite = await DatabaseProvide.Database.GetAsync<MyFavorite>(myFavoriteId);
            myFavorite.MusicCount = myFavorite.MusicCount + 1;
            if (myFavorite.ImageUrl.IsEmpty())
            {
                var musicDetail = await DatabaseProvide.Database.GetAsync<MusicDetail>(_music.Id);
                myFavorite.ImageUrl = musicDetail.ImageUrl;
            }

            await DatabaseProvide.Database.UpdateAsync(myFavorite);
            //TODO 这里做个提示？
            await Navigation.PopPopupAsync();
        }
        private async void BindingMyFavoriteList()
        {
            await Task.Run(() =>
            {
                _myModel.MyFavoriteList.Clear();
                var myFavoriteList = DatabaseProvide.Database.Table<MyFavorite>().ToListAsync().Result;
                foreach (var myFavorite in myFavoriteList)
                {
                    _myModel.MyFavoriteList.Add(new MyFavoriteViewModel()
                    {
                        Id = myFavorite.Id,
                        Name = myFavorite.Name,
                        MusicCount = myFavorite.MusicCount,
                        ImageUrl = myFavorite.ImageUrl
                    });
                }
            });
        }

        private async void BtnAddMyFavorite_Clicked(object sender, EventArgs e)
        {
            _addMyFavoritePage.Initialize();
            await Navigation.PushPopupAsync(_addMyFavoritePage);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Task.Run(() =>
            {
                MyFavoriteViewModel myFavoriteView = e.CurrentSelection[0] as MyFavoriteViewModel;
                AddMusicToMyFavorite(myFavoriteView.Id);
            });
        }
    }
}