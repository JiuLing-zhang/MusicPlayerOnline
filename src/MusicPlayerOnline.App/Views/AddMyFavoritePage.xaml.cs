using System;
using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
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