using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.ViewModelApp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingPage : ContentPage
    {
        private readonly PlayingPageViewModel _myModel = new PlayingPageViewModel();
        public PlayingPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            this.Appearing += (sender, args) =>
            {
                RefreshPage();
            };
        }

        private void RefreshPage()
        {

        }
    }
}