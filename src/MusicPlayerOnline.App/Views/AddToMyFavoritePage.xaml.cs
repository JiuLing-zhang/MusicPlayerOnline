using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToMyFavoritePage : ContentPage
    {
        private readonly AddToMyFavoritePageViewModel _myModel = new AddToMyFavoritePageViewModel();
        public AddToMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
    }
}