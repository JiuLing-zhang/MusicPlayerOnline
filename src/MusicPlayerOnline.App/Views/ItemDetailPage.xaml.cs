using System.ComponentModel;
using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}