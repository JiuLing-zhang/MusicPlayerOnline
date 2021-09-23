using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToMyFavoritePage : PopupPage
    {
        private readonly AddToMyFavoritePageViewModel _myModel = new AddToMyFavoritePageViewModel();
        public Action AddFinished;
        public AddToMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
        public void Initialize()
        {
            BindingMyFavoriteList();
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
    }
}