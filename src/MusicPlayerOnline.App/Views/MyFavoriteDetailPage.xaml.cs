using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.App.ViewModels;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
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
    }
}