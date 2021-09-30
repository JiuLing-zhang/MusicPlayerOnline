using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultPage : ContentPage
    {
        private readonly SearchResultPageViewModel _myModel = new SearchResultPageViewModel();
        public SearchResultPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
    }
}