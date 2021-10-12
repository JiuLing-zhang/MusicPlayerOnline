using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        private readonly SettingPageViewModel _myModel = new SettingPageViewModel();
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
    }
}