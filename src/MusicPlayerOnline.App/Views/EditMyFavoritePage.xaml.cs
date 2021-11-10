using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
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