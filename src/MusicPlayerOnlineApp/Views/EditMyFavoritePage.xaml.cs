using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMyFavoritePage : ContentPage
    {
        private readonly EditMyFavoritePageViewModel _myModel = new EditMyFavoritePageViewModel();
        public EditMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
    }
}