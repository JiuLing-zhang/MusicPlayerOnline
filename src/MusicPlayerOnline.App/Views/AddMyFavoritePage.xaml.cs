using System;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMyFavoritePage : ContentPage
    {
        private readonly AddMyFavoritePageViewModel _myModel = new AddMyFavoritePageViewModel();
        public AddMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
    }
}