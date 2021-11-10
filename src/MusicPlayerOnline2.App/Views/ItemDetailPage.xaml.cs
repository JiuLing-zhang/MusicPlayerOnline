using MusicPlayerOnlineApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Views
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