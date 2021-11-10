using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFavoritePage : ContentPage
    {
        private readonly MyFavoritePageViewModel _myModel = new MyFavoritePageViewModel();
        public MyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            this.Appearing += (_, __) =>
            {
                _myModel.OnAppearing();
            };
        }
    }
}