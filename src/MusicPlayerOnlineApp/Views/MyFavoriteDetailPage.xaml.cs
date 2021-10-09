using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFavoriteDetailPage : ContentPage
    {
        private readonly MyFavoriteDetailPageViewModel _myModel = new MyFavoriteDetailPageViewModel();
        public MyFavoriteDetailPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
        private async void MyFavoriteDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}